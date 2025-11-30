using UnityEngine;

public class TestAddItem : MonoBehaviour
{
    public void OnClickTest()
    {
        // Cộng 1000 vàng
        ResourceManager.Instance.Add(ResourceType.Gold, 1000);
    }
}