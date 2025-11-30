using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmoutSell : MonoBehaviour
{
    public List<TextMeshProUGUI> texts;
    public List<GameObject> images;
    public List<TextMeshProUGUI> priceTexts;
    public Slider amountSlider;
    public TextMeshProUGUI goldText;
    private int currentMaxAmount = 0;
    private ObjectPool.PooledItem selectedItem;
    public ObjectPool objectPool;

    private void OnEnable()
    {
        ButtonSell.OnButtonClickedEvent += OnItemButtonClicked;
        if (amountSlider != null)
            amountSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDisable()
    {
        ButtonSell.OnButtonClickedEvent -= OnItemButtonClicked;
        if (amountSlider != null)
            amountSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnItemButtonClicked(GameObject buttonClicked, int slotIndex)
    {
        Transform parent = buttonClicked.transform.parent;
        if (parent.childCount < 5) return;

        Image img = parent.GetChild(2).GetComponent<Image>();
        TextMeshProUGUI txt = parent.GetChild(3).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI priceTxt = parent.GetChild(4).GetComponent<TextMeshProUGUI>();

        if (img != null)
        {
            foreach (var go in images)
            {
                Image imageComp = go.GetComponent<Image>();
                if (imageComp == null) continue;
                imageComp.sprite = img.sprite;
                go.SetActive(true);
            }
        }

        if (txt != null)
        {
            foreach (var t in texts)
                t.text = txt.text;

            if (int.TryParse(texts[0].text, out int amount))
            {
                currentMaxAmount = amount;
                if (amountSlider != null)
                {
                    amountSlider.maxValue = amount;
                    amountSlider.minValue = 1;
                    amountSlider.value = amount;
                }
            }
        }

        if (priceTxt != null)
        {
            string priceStr = priceTxt.text;
            foreach (var p in priceTexts)
                p.text = priceStr;
        }

        UpdateGoldText();

        selectedItem = null;
        foreach (var item in objectPool.GetActiveObjects())
        {
            if (item.obj == parent.gameObject)
            {
                selectedItem = item;
                break;
            }
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (texts.Count == 0 || selectedItem == null) return;
        int v = Mathf.RoundToInt(value);
        texts[0].text = v.ToString();
        UpdateGoldText();
    }

    private void UpdateGoldText()
    {
        if (texts.Count == 0 || priceTexts.Count == 0 || goldText == null || selectedItem == null) return;
        if (!int.TryParse(texts[0].text, out int quantity)) return;
        int totalGold = quantity * selectedItem.price;
        goldText.text = totalGold.ToString();
    }

    public void SellItems()
    {
        if (selectedItem == null) return;
        if (!int.TryParse(texts[0].text, out int sellAmount)) return;
        if (sellAmount <= 0) return;

        // Trừ số lượng vật phẩm trong PlayerInventory
        PlayerInventory.Instance.RemoveItem(selectedItem.type, sellAmount);

        // Cập nhật ObjectPool
        selectedItem.quantity -= sellAmount;
        if (selectedItem.quantity <= 0)
        {
            selectedItem.obj.SetActive(false);
        }
        else
        {
            TextMeshProUGUI qtyText = selectedItem.obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            if (qtyText != null) qtyText.text = selectedItem.quantity.ToString();
        }

        // Cộng vàng
        int totalGold = sellAmount * selectedItem.price;
        ResourceManager.Instance.Add(ResourceType.Gold, totalGold);

        // Reset slider và text
        if (amountSlider != null)
        {
            amountSlider.value = 0;
            amountSlider.maxValue = currentMaxAmount;
        }

        texts[0].text = selectedItem.quantity.ToString();
        UpdateGoldText();
    }
}