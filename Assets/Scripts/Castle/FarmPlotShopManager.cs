using UnityEngine;

public class FarmPlotShopManager : MonoBehaviour
{
    public static FarmPlotShopManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ĐĂNG KÝ SHOP RUỘNG KHI VÀO GAME
    public void RegisterShop(FarmPlotShopTrigger shop)
    {
        shop.LoadUnlockedPlots(); // Load lại số ô đã mở từ ResourceManager
    }

    // HÀM MÀY ĐANG TÌM ĐÂY NÈ – DÁN VÀO ĐÂY LÀ CHẠY NGON NGAY!!!
    public static void TryBuyCurrentPlot()
    {
        var shop = FarmPlotShopTrigger.CurrentShop;
        if (shop == null || shop.ShopData == null) return;

        ResourceType plotType = shop.ShopData.unlockedPlotResourceType; // Ví dụ: UnlockedFarmPlots
        int currentCount = ResourceManager.Instance.Get(plotType);
        int nextPrice = (currentCount + 1) * shop.ShopData.price;

        if (ResourceManager.Instance.Get(ResourceType.Gold) >= nextPrice)
        {
            ResourceManager.Instance.TrySpend(ResourceType.Gold, nextPrice);

            // TĂNG SỐ Ô ĐÃ MỞ – TỰ ĐỘNG LƯU VÀ HIỆN UI
            ResourceManager.Instance.Add(plotType, 1);

            // BẬT Ô RUỘNG TIẾP THEO TRONG SCENE
            shop.UnlockNextPlot();

            // CẬP NHẬT GIÁ TIỀN VÀ NÚT MUA
            UpdateCurrentShopUI();
        }
    }

    // CẬP NHẬT UI TRONG PANEL SHOP
    public static void UpdateCurrentShopUI()
    {
        var shop = FarmPlotShopTrigger.CurrentShop;
        if (shop == null) return;

        if (shop.BuyButton != null)
        {
            ResourceType plotType = shop.ShopData.unlockedPlotResourceType;
            int currentCount = ResourceManager.Instance.Get(plotType);
            int nextPrice = (currentCount + 1) * shop.ShopData.price;

            bool canAfford = ResourceManager.Instance.Get(ResourceType.Gold) >= nextPrice;
            shop.BuyButton.interactable = canAfford;
        }

        // Cập nhật text level trong panel (nếu có)
        if (CommonUIPanel.Instance != null && CommonUIPanel.Instance.gameObject.activeInHierarchy)
        {
            int currentLevel = ResourceManager.Instance.Get(shop.ShopData.unlockedPlotResourceType);
            CommonUIPanel.Instance.UpdateLevelText(currentLevel, currentLevel + 1);
        }
    }
}