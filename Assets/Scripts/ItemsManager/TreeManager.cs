using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [Header("Select Marker")]
    public GameObject selectMarker;

    [Header("Vị trí spawn gỗ (tùy chọn)")]
    public Transform spawnPoint;

    private bool isPlayerInside = false;
    private float chopTimer = 0f;
    [Header("Thời gian tự động chặt (giây)")]
    public float autoChopTime = 1.5f; // đứng 1.5 giây là chặt luôn

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            selectMarker.SetActive(true);
            chopTimer = 0f; // reset timer
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            selectMarker.SetActive(false);
            chopTimer = 0f;
        }
    }

    private void Update()
    {
        if (isPlayerInside)
        {
            chopTimer += Time.deltaTime;

            // ĐỦ THỜI GIAN → TỰ ĐỘNG CHẶT LUÔN!!!
            if (chopTimer >= autoChopTime)
            {
                Chop();
            }
        }
    }

    public void Chop()
    {
        if (!isPlayerInside) return;
        isPlayerInside = false;

        Die();
        selectMarker.SetActive(false);
    }
    private void Die()
    {
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;

        // Rơi đúng 1 khúc gỗ, lệch nhẹ cho đẹp
        Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
        ItemPickupPool.Instance.Get(ItemType.Wood, spawnPos + offset, 1);

        gameObject.SetActive(false);
    }

    // Reset khi tái sử dụng cây (nếu dùng pool cây)
    private void OnEnable()
    {
        selectMarker.SetActive(false);
        isPlayerInside = false;
        chopTimer = 0f;
    }
}