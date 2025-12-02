// ProductionSpeedManager.cs – BẢN MỚI, RIÊNG CHO TỪNG LOẠI ĐỘNG VẬT
using UnityEngine;
using System.Collections.Generic;

public class ProductionSpeedManager : MonoBehaviour
{
    public static ProductionSpeedManager Instance { get; private set; }

    // Lưu % tăng tốc riêng cho từng loại động vật (dùng tên asset làm key)
    private Dictionary<string, int> speedBonusByType = new Dictionary<string, int>();

    private const string SAVE_PREFIX = "SpeedBonus_";

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

   
    private const int MAX_BONUS_PERCENT = 99; 

    public void UpgradeSpeedForType(string typeKey)
    {
        int current = GetSpeedBonusForType(typeKey);

       
        if (current >= MAX_BONUS_PERCENT)
        {
            
            return;
        }

        speedBonusByType[typeKey] = current + 1;
        PlayerPrefs.SetInt(SAVE_PREFIX + typeKey, current + 1);
        PlayerPrefs.Save();

       
        if (CommonUIPanel.Instance != null && CommonUIPanel.Instance.gameObject.activeInHierarchy)
        {
            var currentShop = ShopTrigger.CurrentShop;
            if (currentShop != null && GetTypeKey(currentShop) == typeKey)
            {
                CommonUIPanel.Instance.UpdateProductionSpeedText();
            }
        }
    }

    // Lấy % tăng tốc của loại này
    public int GetSpeedBonusForType(string typeKey)
    {
        if (speedBonusByType.TryGetValue(typeKey, out int bonus))
            return bonus;

        // Nếu chưa có trong RAM → load từ PlayerPrefs
        bonus = PlayerPrefs.GetInt(SAVE_PREFIX + typeKey, 0);
        speedBonusByType[typeKey] = bonus;
        return bonus;
    }

    // Lấy thời gian thực tế cho 1 con vật cụ thể
    public float GetActualFillTime(float baseFillTime, string typeKey)
    {
        int bonus = GetSpeedBonusForType(typeKey);
        return baseFillTime / (1f + bonus / 100f);
    }

    public int GetCurrentBonusPercent(string typeKey) => GetSpeedBonusForType(typeKey);
    public int GetNextBonusPercent(string typeKey) => GetSpeedBonusForType(typeKey) + 1;

    public static string GetTypeKey(MonoBehaviour shop)
    {
        if (shop == null) return "Unknown";

        if (shop is ShopTrigger animalShop)
            return animalShop.ShopData.name;

        if (shop is FarmPlotShopTrigger farmShop)
            return farmShop.ShopData.name;

        return shop.name; 
    }
}