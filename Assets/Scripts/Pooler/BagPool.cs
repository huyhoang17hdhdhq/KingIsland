using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagPool : MonoBehaviour
{
    [Header("=== PREFAB RIÊNG CHO TÚI ĐỒ ===")]
    [SerializeField] private GameObject bagItemPrefab;     // Prefab riêng cho túi (có Image + Text)
    [SerializeField] private Transform contentParent;      // Content của túi

    [Header("=== DỮ LIỆU TỪ ObjectPool (Shop) ===")]
    [SerializeField] private ObjectPool shopObjectPool;     // Kéo ObjectPool của shop vào

    [Header("=== CHILD TRONG PREFAB TÚI ===")]
    [SerializeField] private int iconChildIndex = 0;       // Image trong prefab túi
    [SerializeField] private int quantityChildIndex = 1;   // Text trong prefab túi

    [Header("=== SỐ LƯỢNG TẠO TRƯỚC ===")]
    [SerializeField] private int preloadAmount = 20;

    private List<GameObject> pool = new List<GameObject>();
    private List<GameObject> activeBagItems = new List<GameObject>();

    private void Awake()
    {
        // Tạo pool riêng cho túi
        for (int i = 0; i < preloadAmount; i++)
        {
            GameObject obj = Instantiate(bagItemPrefab, contentParent);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    private void Start()
    {
        UpdateBagDisplay();
    }

    private void Update()
    {
        if (shopObjectPool != null)
            UpdateBagDisplay();
    }

    private GameObject GetFromPool()
    {
        foreach (var obj in pool)
            if (!obj.activeInHierarchy) return obj;

        GameObject newObj = Instantiate(bagItemPrefab, contentParent);
        pool.Add(newObj);
        return newObj;
    }

    private void UpdateBagDisplay()
    {
        // Ẩn tất cả ô cũ
        foreach (var item in activeBagItems)
            item.SetActive(false);
        activeBagItems.Clear();

        if (shopObjectPool == null) return;

        var activeItems = shopObjectPool.GetActiveObjects();

        foreach (var pooled in activeItems)
        {
            if (pooled.obj == null || pooled.quantity <= 0) continue;

            GameObject bagItem = GetFromPool();
            bagItem.SetActive(true);

            // Lấy icon + số lượng từ ObjectPool (shop)
            var sourceIcon = pooled.obj.transform.GetChild(2).GetComponent<Image>(); // icon ở shop
            var sourceQty = pooled.quantity;

            // Gán vào prefab túi
            var bagIcon = bagItem.transform.GetChild(iconChildIndex).GetComponent<Image>();
            var bagText = bagItem.transform.GetChild(quantityChildIndex).GetComponent<TextMeshProUGUI>();

            if (bagIcon != null && sourceIcon != null)
                bagIcon.sprite = sourceIcon.sprite;

            if (bagText != null)
                bagText.text = sourceQty.ToString();

            activeBagItems.Add(bagItem);
        }
    }
}