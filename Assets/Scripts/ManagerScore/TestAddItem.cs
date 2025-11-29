using UnityEngine;

public class TestAddItem : MonoBehaviour
{
    //public ItemType testType;
    //public int amount = 1;

    //public void AddItemTest()
    //{
    //    PlayerInventory.Instance.AddItem(testType, amount);
    //}
    public void OnClickTest()
    {
        GoldManager.Instance.AddGold(1000);
    }
}
