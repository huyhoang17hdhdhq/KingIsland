using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Data", menuName = "Shop/Event Info")]
public class EventInfor : ScriptableObject
{
    public string nameIsland = "Cửa hàng Bò";
    public string castle = "Chuồng Bò VIP";
    public string productionRate = "100 sữa / ngày";

    public int price = 100;
    public int diamond = 50;
    public string description = "Mua bò để farm sữa tự động!";

    public GameObject prefabToSpawn;

    // Dành cho mở ô đất (rau củ)
    public ResourceType unlockedPlotResourceType = ResourceType.Gold;

    // QUAN TRỌNG NHẤT: DÀNH CHO ĐỘNG VẬT VÀ MỞ Ô ĐẤT
    [Header("=== LOẠI RESOURCE TĂNG KHI MUA / MỞ Ô ===")]
    public ResourceType resourceToIncrease;
}