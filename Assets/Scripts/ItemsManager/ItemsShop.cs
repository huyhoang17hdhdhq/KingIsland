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
    private void OnTriggerExit2D(Collider2D other)  
    {
        if (other.CompareTag("Player"))
        {
            shopItems.SetActive(false);
            Debug.Log("đóng shop items");
        }
    }



}