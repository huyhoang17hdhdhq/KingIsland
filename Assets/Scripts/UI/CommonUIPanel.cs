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
       
        if (nameIslandText != null) nameIslandText.text = info.nameIsland;
        if (castleText != null) castleText.text = info.castle;
        if (productionRateText != null) productionRateText.text = info.productionRate;

      
        int currentPrice = 0;

       
        var farmShop = FarmPlotShopTrigger.CurrentShop;
        if (farmShop != null)
        {
            int count = ResourceManager.Instance.Get(farmShop.ShopData.unlockedPlotResourceType);
            currentPrice = (count + 1) * info.price;
        }
        
        else
        {
            currentPrice = AnimalShopManager.GetCurrentPrice();
        }

        if (priceText != null)
            priceText.text = currentPrice.ToString();

        
        int currentLevel = 0;

        if (farmShop != null)
        {
            currentLevel = ResourceManager.Instance.Get(farmShop.ShopData.unlockedPlotResourceType);
        }
        else
        {
            var animalShop = ShopTrigger.CurrentShop;
            if (animalShop != null)
                currentLevel = animalShop.GetCurrentLevel();
        }

        UpdateLevelText(currentLevel, currentLevel + 1);

        
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
        string typeKey = "";

        var farmShop = FarmPlotShopTrigger.CurrentShop;
        var animalShop = ShopTrigger.CurrentShop;

        if (farmShop != null)
        {
            typeKey = ProductionSpeedManager.GetTypeKey(farmShop);
        }
        else if (animalShop != null)
        {
            typeKey = ProductionSpeedManager.GetTypeKey(animalShop);
        }

        if (string.IsNullOrEmpty(typeKey)) return;

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

            var btnText = upgradeSpeedButton.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
                btnText.text = isMax ? "MAX" : "Tăng tốc";
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
    
   