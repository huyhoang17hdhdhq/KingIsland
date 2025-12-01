using UnityEngine;

public class ItemsShop : MonoBehaviour
{
    public GameObject shopItems;

    private void OnTriggerEnter2D(Collider2D other)  
    {
        if (other.CompareTag("Player"))
        {
            shopItems.SetActive(true);
            Debug.Log("mở shop items");
        }
    }

   
    public void CloseShopItems()
    {
        if (shopItems != null)
            shopItems.SetActive(false);
    }
}