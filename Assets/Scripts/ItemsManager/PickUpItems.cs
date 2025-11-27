using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemType itemType; 
    public int amount = 1;    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           
            PlayerInventory.Instance.AddItem(itemType, amount);

           
           Destroy (gameObject,1f);
        }
    }
}
