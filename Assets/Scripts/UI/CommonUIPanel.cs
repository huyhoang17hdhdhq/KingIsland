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
    public TextMeshProUGUI diamondText;

    public Button upgradeSpeedButton;

    [SerializeField] private GameObject panelContent;

    

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        
        if (panelContent != null)
            panelContent.SetActive(false);
    }

    public void Show(EventInfor info)
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

        if (panelContent != null)
            panelContent.SetActive(true);
    }

    


    public void UpdatePriceText(int price)
    {
        if (priceText != null)
            priceText.text = price.ToString();
    }
    public void UpdateDiamondText(int diamond)
    {
        if (diamondText != null)
            diamondText.text = diamond.ToString();
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

    if (panelContent != null)
        panelContent.SetActive(false);
    }


    public void UpdateProductionSpeedText()
    {
        string typeKey = GetCurrentShopSpeedType();
        if (string.IsNullOrEmpty(typeKey) || typeKey == "Unknown")
        {
            if (productionlNowText) productionlNowText.text = "0%";
            if (productionNextText) productionNextText.text = "1%";
            if (diamondText) diamondText.text = "0";
            return;
        }

        int currentBonus = ProductionSpeedManager.Instance.GetCurrentBonusPercent(typeKey);
        int nextBonus = ProductionSpeedManager.Instance.GetNextBonusPercent(typeKey);
        int nextDiamondCost = ProductionSpeedManager.Instance.GetNextUpgradeDiamondCost(typeKey);

        if (productionlNowText != null)
            productionlNowText.text = $"{currentBonus}%";
        if (productionNextText != null)
            productionNextText.text = $"{nextBonus}%";

        
        if (diamondText != null)
            diamondText.text = nextDiamondCost.ToString();

        if (upgradeSpeedButton != null)
        {
            bool isMax = currentBonus >= 99;
            bool canAfford = ResourceManager.Instance.Get(ResourceType.Diamond) >= nextDiamondCost;

            upgradeSpeedButton.interactable = !isMax && canAfford;

            var btnText = upgradeSpeedButton.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                if (isMax) btnText.text = "MAX";
                else if (!canAfford) btnText.text = "Không đủ";
                else btnText.text = "Tăng tốc";
            }
        }
    }
    public void OnUpgradeSpeedClicked()
    {
        string typeKey = GetCurrentShopSpeedType();
        if (string.IsNullOrEmpty(typeKey) || typeKey == "Unknown") return;

        if (ProductionSpeedManager.Instance.TryUpgradeSpeedWithDiamond(typeKey))
        {

            UpdateProductionSpeedText();
        }
        else
        {
            // Không đủ Diamond hoặc đã max
            Debug.Log("KHÔNG ĐỦ DIAMOND HOẶC ĐÃ MAX!");
            // Có thể hiện thông báo "Không đủ Kim Cương!"
        }
    }

    private string GetCurrentShopSpeedType()
    {
        if (ShopTrigger.CurrentShop != null)
            return ShopTrigger.CurrentShop.GetProductionSpeedType();

        if (FarmPlotShopTrigger.CurrentShop != null)
            return FarmPlotShopTrigger.CurrentShop.GetProductionSpeedType();

        return "Unknown";
    }

}
    
   