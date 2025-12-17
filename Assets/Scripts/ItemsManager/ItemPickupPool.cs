using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RewardPool
{
    public ItemType type;
    public PickUpItems prefab;
    public int poolSize = 50;
}

public class ItemPickupPool : MonoBehaviour
{
    public static ItemPickupPool Instance;

    [Header("Danh sách tất cả loại vật phẩm")]
    public List<RewardPool> rewardPools = new List<RewardPool>();

    private Dictionary<ItemType, Queue<PickUpItems>> pools = new Dictionary<ItemType, Queue<PickUpItems>>();

    private void Awake()
    {
        Instance = this;

        foreach (var rp in rewardPools)
        {
            var queue = new Queue<PickUpItems>();
            for (int i = 0; i < rp.poolSize; i++)
            {
                PickUpItems item = Instantiate(rp.prefab, transform);
                item.gameObject.SetActive(false);
                queue.Enqueue(item);
            }
            pools[rp.type] = queue;
        }
    }

    public PickUpItems Get(ItemType type, Vector3 position, int amount = 1)
    {
        if (!pools.ContainsKey(type) || pools[type].Count == 0)
        {
            // Tự động tạo thêm nếu hết
            var rp = rewardPools.Find(x => x.type == type);
            if (rp != null)
            {
                PickUpItems item = Instantiate(rp.prefab, transform);
                item.gameObject.SetActive(false);
                pools[type].Enqueue(item);
            }
        }

        var pickup = pools[type].Dequeue();
        pickup.Setup(type, amount, position);
        return pickup;
    }

    public void ReturnToPool(PickUpItems item)
    {
        item.StopAllCoroutines();
        item.gameObject.SetActive(false);
        item.transform.SetParent(transform);
        pools[item.itemType].Enqueue(item);
    }
}