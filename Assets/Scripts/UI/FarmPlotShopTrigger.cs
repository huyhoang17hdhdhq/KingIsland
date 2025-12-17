using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CropType
{
    Rice,
    Wheat,
    Corn,
    Potato,
    Vegetable
}

public class FarmPlotShopTrigger : MonoBehaviour
{
    [Header("=== CÀI ĐẶT SHOP RUỘNG ===")]
    [SerializeField] private EventInfor shopData;
    [SerializeField] private GameObject interactButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private string customButtonText = "Mở Shop Rau";
    [Header("=== LOẠI CÂY (BẮT BUỘC CHỌN) ===")]
    [SerializeField] private CropType cropType;

    [Header("=== CHA CỦA TẤT CẢ Ô RUỘNG LOẠI NÀY ===")]
    [SerializeField] private Transform farmPlotsParent;

    [Header("=== NÚT MUA THÊM Ô TRONG SHOP (NẾU CÓ) ===")]
    [SerializeField] private Button buyButton;

    private static FarmPlotShopTrigger currentShop;
    [SerializeField] private string productionSpeedType = "Cow";

    private ResourceType SaveResourceType
    {
        get
        {
            return cropType switch
            {
                CropType.Rice => ResourceType.UnlockedRicePlots,
                CropType.Wheat => ResourceType.UnlockedWheatPlots,
                CropType.Corn => ResourceType.UnlockedCornPlots,
                CropType.Potato => ResourceType.UnlockedPotatoPlots,
                CropType.Vegetable => ResourceType.UnlockedVegetablePlots,
                _ => ResourceType.UnlockedWheatPlots
            };
        }
    }

    private void Start()
    {
        if (FarmPlotShopManager.Instance != null)
            FarmPlotShopManager.Instance.RegisterShop(this);

        LoadUnlockedPlots();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        currentShop = this;
        if (interactButton != null) interactButton.SetActive(true);
        if (buttonText != null) buttonText.text = customButtonText;

        FarmPlotShopManager.UpdateCurrentShopUI();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (currentShop == this)
        {
            currentShop = null;
            if (interactButton != null) interactButton.SetActive(false);
        }
    }

    public static void OpenCurrentShop()
    {
        if (currentShop != null && currentShop.shopData != null)
        {
            CommonUIPanel.Instance.Show(currentShop.shopData);
            FarmPlotShopManager.UpdateCurrentShopUI();

            if (currentShop.interactButton != null)
                currentShop.interactButton.SetActive(false);
        }
    }

    public int GetUnlockedPlotCount()
    {
        if (farmPlotsParent == null) return 0;
        int count = 0;
        foreach (Transform child in farmPlotsParent)
        {
            if (child.gameObject.activeSelf) count++;
        }
        return count;
    }

    public void UnlockNextPlot()
    {
        if (farmPlotsParent == null) return;

        int currentCount = GetUnlockedPlotCount();
        if (currentCount < farmPlotsParent.childCount)
        {
            farmPlotsParent.GetChild(currentCount).gameObject.SetActive(true);
            ResourceManager.Instance.Set(SaveResourceType, currentCount + 1); 
            Debug.Log($"ĐÃ MỞ Ô {cropType} THỨ {currentCount + 1}!");
        }
    }

    public void LoadUnlockedPlots()
    {
        if (farmPlotsParent == null) return;

        int unlocked = ResourceManager.Instance.Get(SaveResourceType);
        for (int i = 0; i < farmPlotsParent.childCount; i++)
        {
            farmPlotsParent.GetChild(i).gameObject.SetActive(i < unlocked);
        }
    }
    public string GetProductionSpeedType()
    {
        return productionSpeedType; 
    }

    public EventInfor ShopData => shopData;
    public Button BuyButton => buyButton;
    public static FarmPlotShopTrigger CurrentShop => currentShop;

    private void OnDestroy()
    {
        if (currentShop == this) currentShop = null;
    }
}