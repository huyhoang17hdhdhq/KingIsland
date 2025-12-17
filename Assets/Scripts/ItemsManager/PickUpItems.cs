using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    public ItemType itemType;
    public int amount = 1;
    private bool isPickedUp = false;

   
    [Header("Thời gian chờ sau khi nhặt (giây)")]
    public float delayBeforeReturn = 1f;

    public void Setup(ItemType type, int qty, Vector3 position)
    {
        itemType = type;
        amount = qty;
        transform.position = position;
        gameObject.SetActive(true);
        isPickedUp = false;

        CancelInvoke(); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPickedUp || !other.CompareTag("Player")) return;

        isPickedUp = true;
        PlayerInventory.Instance.AddItem(itemType, amount);

        MusicManager.Instance.LootItemsSound();


        QuestManager.Instance.ReportHarvest(itemType, amount);

        Invoke(nameof(ReturnToPool), delayBeforeReturn);
    }

    private void ReturnToPool()
    {
        ItemPickupPool.Instance.ReturnToPool(this);
    }
}