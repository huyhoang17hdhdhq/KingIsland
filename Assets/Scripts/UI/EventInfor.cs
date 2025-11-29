using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Data", menuName = "Shop/Event Info")]
public class EventInfo : ScriptableObject
{
    public string nameIsland = "Cửa hàng Bò";
    public string castle = "Chuồng Bò VIP";
    public string productionRate = "100 sữa / ngày";

    
   
    public int price = 100;
    public string description = "Mua bò để farm sữa tự động!";
    public GameObject prefabToSpawn;
    
}