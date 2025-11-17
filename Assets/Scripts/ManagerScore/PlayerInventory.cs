using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public ItemUIManager uiManager;
    public ObjectPool objectPool;

    private Dictionary<ItemType, int> itemCounts = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        foreach (var data in uiManager.items)
        {
            itemCounts[data.type] = PlayerPrefs.GetInt(data.type.ToString(), 0);
            uiManager.UpdateItemUI(data.type, itemCounts[data.type]);
        }

        UpdateAllPoolObjects();
    }

    public void AddItem(ItemType type, int amount)
    {
        ChangeValue(type, amount);
    }

    public void RemoveItem(ItemType type, int amount)
    {
        ChangeValue(type, -amount);
    }

    private void ChangeValue(ItemType type, int amount)
    {
        int oldValue = itemCounts.ContainsKey(type) ? itemCounts[type] : 0;
        int newValue = Mathf.Max(0, oldValue + amount);
        itemCounts[type] = newValue;

        PlayerPrefs.SetInt(type.ToString(), newValue);
        PlayerPrefs.Save();

        uiManager.UpdateItemUI(type, newValue);
        UpdateAllPoolObjects();
    }

    private void UpdateAllPoolObjects()
    {
        objectPool.UpdateObjects(itemCounts, uiManager);
    }
}
