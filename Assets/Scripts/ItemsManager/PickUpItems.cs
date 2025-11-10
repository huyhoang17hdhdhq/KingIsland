using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    [Header("Tên Layer vật phẩm")]
    public string itemLayerName = "items"; 

    private void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.gameObject.layer == LayerMask.NameToLayer(itemLayerName))
        {
            
            Destroy(other.gameObject,1);
        }
    }
}
