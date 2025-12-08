using UnityEngine;
using System.Collections.Generic;

public class ProductionSpeedManager : MonoBehaviour
{
    public static ProductionSpeedManager Instance { get; private set; }

    private Dictionary<string, int> speedBonusByType = new Dictionary<string, int>();
    private const string SAVE_PREFIX = "SpeedBonus_";
    private const int MAX_BONUS_PERCENT = 99;

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

    public bool TryUpgradeSpeedWithDiamond(string typeKey)
    {
        int currentLevel = GetSpeedBonusForType(typeKey);

        if (currentLevel >= MAX_BONUS_PERCENT)
            return false;

       
        EventInfo shopData = GetCurrentShopData();
        if (shopData == null || shopData.diamond <= 0)
            return false;

        int requiredDiamond = (currentLevel + 1) * shopData.diamond;

     
        if (!ResourceManager.Instance.TrySpend(ResourceType.Diamond, requiredDiamond))
            return false;

       
        speedBonusByType[typeKey] = currentLevel + 1;
        PlayerPrefs.SetInt(SAVE_PREFIX + typeKey, currentLevel + 1);
        PlayerPrefs.Save();

        QuestManager.Instance.ReportUpgradeSpeed(typeKey, 1);

        CommonUIPanel.Instance?.UpdateProductionSpeedText();

        Debug.Log($"NÂNG TỐC ĐỘ THÀNH CÔNG: {typeKey} → +{currentLevel + 1}% (tốn {requiredDiamond} Diamond)");
        return true;
    }

   
    public void UpgradeSpeedForType(string typeKey)
    {
        int current = GetSpeedBonusForType(typeKey);
        if (current >= MAX_BONUS_PERCENT) return;

        speedBonusByType[typeKey] = current + 1;
        PlayerPrefs.SetInt(SAVE_PREFIX + typeKey, current + 1);
        PlayerPrefs.Save();

        CommonUIPanel.Instance?.UpdateProductionSpeedText();
    }

    public int GetSpeedBonusForType(string typeKey)
    {
        if (speedBonusByType.TryGetValue(typeKey, out int bonus))
            return bonus;

        bonus = PlayerPrefs.GetInt(SAVE_PREFIX + typeKey, 0);
        speedBonusByType[typeKey] = bonus;
        return bonus;
    }

    public float GetActualFillTime(float baseFillTime, string typeKey)
    {
        int bonus = GetSpeedBonusForType(typeKey);
        return baseFillTime / (1f + bonus / 100f);
    }

    public int GetCurrentBonusPercent(string typeKey) => GetSpeedBonusForType(typeKey);
    public int GetNextBonusPercent(string typeKey) => GetSpeedBonusForType(typeKey) + 1;

    // LẤY GIÁ DIAMOND CHO LẦN NÂNG TIẾP THEO
    public int GetNextUpgradeDiamondCost(string typeKey)
    {
        var shopData = GetCurrentShopData();
        if (shopData == null || shopData.diamond <= 0) return 99999;

        int nextLevel = GetSpeedBonusForType(typeKey) + 1;
        return nextLevel * shopData.diamond;
    }

    // HÀM HỖ TRỢ: LẤY EventInfo CỦA SHOP ĐANG MỞ (HỖ TRỢ CẢ 2 LOẠI SHOP)
    private EventInfo GetCurrentShopData()
    {
        if (ShopTrigger.CurrentShop != null)
            return ShopTrigger.CurrentShop.ShopData;

        if (FarmPlotShopTrigger.CurrentShop != null)
            return FarmPlotShopTrigger.CurrentShop.ShopData;

        return null;
    }

    public static string GetTypeKey(MonoBehaviour shop)
    {
        if (shop == null) return "Unknown";
        if (shop is ShopTrigger st) return st.ShopData?.name ?? "Unknown";
        if (shop is FarmPlotShopTrigger fp) return fp.ShopData?.name ?? "Unknown";
        return shop.name;
    }
}