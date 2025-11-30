using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class PooledItem
    {
        public GameObject obj;
        public ItemType type;
        public int quantity;
        public int price;
        public PooledItem(GameObject obj)
        {
            this.obj = obj;
        }
    }
    [Header("Prefab vật phẩm hiển thị")]
    public GameObject itemPrefab;
    [Header("Parent chứa tất cả object bật lên")]
    public Transform poolParent;
    [Header("Số lượng object tạo trước")]
    public int preloadAmount = 20;
    private List<GameObject> pool = new();
    private List<PooledItem> activeObjects = new();
    private void Awake()
    {
        PreloadObjects();
    }
    private void PreloadObjects()
    {
        for (int i = 0; i < preloadAmount; i++)
        {
            GameObject obj = Instantiate(itemPrefab, poolParent);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }
    private GameObject GetObjectFromPool()
    {
        foreach (var obj in pool)
            if (!obj.activeSelf) return obj;
        GameObject newObj = Instantiate(itemPrefab, poolParent);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }

    public void UpdateObjects(Dictionary<ItemType, int> itemCounts, ItemUIManager uiManager)
    {

        foreach (var item in activeObjects)
            item.obj.SetActive(false);
        activeObjects.Clear();
        foreach (var pair in itemCounts)
        {
            if (pair.Value <= 0) continue;
            GameObject obj = GetObjectFromPool();
            obj.SetActive(true);
            var spriteObj = obj.transform.GetChild(2).GetComponent<Image>();
            var textObj = obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            var priceObj = obj.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
            if (spriteObj != null)
                spriteObj.sprite = uiManager.GetItemSprite(pair.Key);
            if (textObj != null)
                textObj.text = pair.Value.ToString();
            if (priceObj != null)
                priceObj.text = uiManager.GetItemPrice(pair.Key).ToString();

            activeObjects.Add(new PooledItem(obj)
            {
                type = pair.Key,
                quantity = pair.Value,
                price = uiManager.GetItemPrice(pair.Key)
            });
        }
    }
    public void SellItem(PooledItem pooledItem, int sellQuantity)
    {
        if (pooledItem == null || sellQuantity <= 0) return;

        int newQuantity = Mathf.Max(0, pooledItem.quantity - sellQuantity);
        pooledItem.quantity = newQuantity;

        var textObj = pooledItem.obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        if (textObj != null)
            textObj.text = newQuantity.ToString();

        if (newQuantity == 0)
            pooledItem.obj.SetActive(false);

        PlayerInventory.Instance.RemoveItem(pooledItem.type, sellQuantity);

        int goldEarned = sellQuantity * pooledItem.price;
        ResourceManager.Instance.Add(ResourceType.Gold, goldEarned);
    }

    public List<PooledItem> GetActiveObjects()
    {
        return activeObjects;
    }
}