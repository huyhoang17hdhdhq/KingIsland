using UnityEngine;

public class TestAddItem : MonoBehaviour
{
    public void OnClickTest()
    {
        
        ResourceManager.Instance.Add(ResourceType.Gold, 1000);
        ResourceManager.Instance.Add(ResourceType.Diamond, 1000);

    }
}