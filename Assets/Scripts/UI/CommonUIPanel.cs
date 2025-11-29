using TMPro;
using UnityEngine;

public class CommonUIPanel : MonoBehaviour
{
    public static CommonUIPanel Instance;
    public TextMeshProUGUI nameIslandText;
    public TextMeshProUGUI castleText;
    public TextMeshProUGUI productionRateText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI levelNowText;   // Level hiện tại
    public TextMeshProUGUI levelNextText;  // Level tiếp theo
    public GameObject closeButton;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Show(EventInfo info)
    {
        nameIslandText.text = info.nameIsland;
        castleText.text = info.castle;
        productionRateText.text = info.productionRate;
        priceText.text = AnimalShopManager.GetCurrentPrice().ToString();

        // Tự động cập nhật level khi mở panel
        var shop = ShopTrigger.CurrentShop;
        if (shop != null)
        {
            int currentLevel = shop.GetCurrentLevel();
            UpdateLevelText(currentLevel, currentLevel + 1);
        }

        gameObject.SetActive(true);
    }

    public void UpdatePriceText(int price)
    {
        if (priceText != null)
            priceText.text = price.ToString();
    }

    // HÀM MỚI: Cập nhật Level
    public void UpdateLevelText(int current, int next)
    {
        if (levelNowText != null)
            levelNowText.text = current.ToString();

        if (levelNextText != null)
            levelNextText.text = next.ToString();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}