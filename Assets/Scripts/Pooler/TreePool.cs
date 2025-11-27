using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePool : MonoBehaviour
{
    [System.Serializable]
    public class PooledTree
    {
        public GameObject prefab;
        public int initialSize = 10;
    }

    [Header("Danh sách prefab cây để tạo pool")]
    public List<PooledTree> treesToPool;

    [Header("Thiết lập Grid Spawn")]
    public int rows = 5;
    public int columns = 5;
    public float spacingX = 2f;
    public float spacingY = 2f;
    public Vector3 gridOffset = Vector3.zero;

    [Header("Thời gian tự hiện lại sau khi bị ẩn (giây)")]
    public float showAfterSeconds = 5f;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary;
    private Dictionary<GameObject, float> hiddenTimers; // lưu thời gian bắt đầu ẩn của cây

    void Awake()
    {
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
        hiddenTimers = new Dictionary<GameObject, float>();

        int totalGridSlots = rows * columns;

        foreach (var tree in treesToPool)
        {
            Queue<GameObject> treePool = new Queue<GameObject>();
            int spawnAmount = Mathf.Min(tree.initialSize, totalGridSlots);

            for (int i = 0; i < spawnAmount; i++)
            {
                Vector3 position = GetGridPosition(i);
                GameObject newTree = Instantiate(tree.prefab, position, Quaternion.identity);
                newTree.transform.SetParent(transform);
                newTree.SetActive(true);

                treePool.Enqueue(newTree);
            }

            poolDictionary.Add(tree.prefab, treePool);
        }
    }

    private Vector3 GetGridPosition(int index)
    {
        int row = index / columns;
        int column = index % columns;

        float posX = column * spacingX;
        float posY = -row * spacingY;

        return transform.position + new Vector3(posX, posY, 0);
    }

    void Update()
    {
        // Lặp tất cả cây trong pool, check xem cây nào bị inactive
        foreach (var kvp in poolDictionary)
        {
            foreach (var tree in kvp.Value)
            {
                if (!tree.activeSelf)
                {
                    if (!hiddenTimers.ContainsKey(tree))
                    {
                        // Bắt đầu đếm thời gian khi thấy cây bị ẩn
                        hiddenTimers[tree] = Time.time;
                    }
                    else
                    {
                        float elapsed = Time.time - hiddenTimers[tree];
                        if (elapsed >= showAfterSeconds)
                        {
                            tree.SetActive(true);
                            hiddenTimers.Remove(tree); // reset timer
                        }
                    }
                }
                else
                {
                    // Nếu cây đang active, xóa timer nếu có
                    if (hiddenTimers.ContainsKey(tree))
                        hiddenTimers.Remove(tree);
                }
            }
        }
    }

    // Lấy cây từ Pool
    public GameObject GetTree(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogError("Prefab không tồn tại trong TreePool: " + prefab.name);
            return null;
        }

        GameObject treeObj;

        if (poolDictionary[prefab].Count > 0)
        {
            treeObj = poolDictionary[prefab].Dequeue();
        }
        else
        {
            treeObj = Instantiate(prefab);
            treeObj.transform.SetParent(transform);
        }

        treeObj.SetActive(true);
        return treeObj;
    }

    // Trả cây về pool (vẫn giữ logic hiện lại tự động)
    public void ReturnTree(GameObject prefab, GameObject treeObj)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning("Prefab không tồn tại trong Pool: " + prefab.name);
            Destroy(treeObj);
            return;
        }

        treeObj.SetActive(false);
        poolDictionary[prefab].Enqueue(treeObj);
        // Không cần coroutine nữa, Update() sẽ quản lý việc bật lại
    }
}
