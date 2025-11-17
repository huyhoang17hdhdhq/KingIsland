using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIManager : MonoBehaviour
{
    [System.Serializable]
    public class ItemUIData
    {
        public ItemType type;
        public Sprite image;

        [Header("Giá bán của vật phẩm")]
        public int price = 0;   

        public List<TextMeshProUGUI> quantityTexts = new();
    }

    [Header("Danh sách UI từng vật phẩm")]
    public List<ItemUIData> items = new();

    private Dictionary<ItemType, ItemUIData> itemDict = new();

    private void Awake()
    {
        foreach (var i in items)
            itemDict[i.type] = i;
    }


    public void UpdateItemUI(ItemType type, int amount)
    {
        if (!itemDict.ContainsKey(type)) return;

        var data = itemDict[type];

        foreach (var txt in data.quantityTexts)
            if (txt != null) txt.text = amount.ToString();

        bool isActive = amount > 0;
        foreach (var txt in data.quantityTexts)
            if (txt != null) txt.transform.parent.gameObject.SetActive(isActive);
    }

   
    public Sprite GetItemSprite(ItemType type)
    {
        return itemDict.ContainsKey(type) ? itemDict[type].image : null;
    }

    
    public int GetItemPrice(ItemType type)
    {
        return itemDict.ContainsKey(type) ? itemDict[type].price : 0;
    }
}
