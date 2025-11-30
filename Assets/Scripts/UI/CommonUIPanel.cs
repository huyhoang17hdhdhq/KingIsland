using UnityEngine;
using UnityEngine.UI;        
using TMPro;
using System.Collections;

public class CommonUIPanel : MonoBehaviour
{
    public static CommonUIPanel Instance;
    public TextMeshProUGUI nameIslandText;
    public TextMeshProUGUI castleText;
    public TextMeshProUGUI productionRateText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI levelNowText;  
    public TextMeshProUGUI levelNextText;
    public TextMeshProUGUI productionlNowText;
    public TextMeshProUGUI productionNextText;

    public Button upgradeSpeedButton;

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

        var shop = ShopTrigger.CurrentShop;
        if (shop != null)
        {
            int currentLevel = shop.GetCurrentLevel();
            UpdateLevelText(currentLevel, currentLevel + 1);
        }

       
        UpdateProductionSpeedText();

        gameObject.SetActive(true);
    }

    public void UpdatePriceText(int price)
    {
        if (priceText != null)
            priceText.text = price.ToString();
    }

    
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
    
    public void UpdateProductionSpeedText()
    {
        var shop = ShopTrigger.CurrentShop;
        if (shop == null) return;

        string typeKey = ProductionSpeedManager.GetTypeKey(shop);
        int currentBonus = ProductionSpeedManager.Instance.GetCurrentBonusPercent(typeKey);
        int nextBonus = ProductionSpeedManager.Instance.GetNextBonusPercent(typeKey);

        if (productionlNowText != null)
            productionlNowText.text = $"{currentBonus}%";

        if (productionNextText != null)
            productionNextText.text = $"{nextBonus}%";

        if (upgradeSpeedButton != null)
        {
            bool isMax = currentBonus >= 99;
            upgradeSpeedButton.interactable = !isMax;
            upgradeSpeedButton.GetComponentInChildren<TextMeshProUGUI>().text = isMax ? "MAX" : "Tăng tốc";
        }
    }
    public void OnUpgradeSpeedClicked()
    {
        var shop = ShopTrigger.CurrentShop;
        if (shop == null) return;

        string typeKey = ProductionSpeedManager.GetTypeKey(shop);
        ProductionSpeedManager.Instance.UpgradeSpeedForType(typeKey);


        UpdateProductionSpeedText();
    }

}
    
   