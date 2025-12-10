using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public delegate void OnInventoryChangedDelegate();
    public static event OnInventoryChangedDelegate OnInventoryChanged;

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
        OnInventoryChanged?.Invoke();
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
        OnInventoryChanged?.Invoke();
    }

    private void UpdateAllPoolObjects()
    {
        objectPool.UpdateObjects(itemCounts, uiManager);
    }
    public Dictionary<ItemType, int> GetAllItems()
    {
        return new Dictionary<ItemType, int>(itemCounts);
    }
}
