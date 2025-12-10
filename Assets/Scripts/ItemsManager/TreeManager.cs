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
    public float autoChopTime = 1.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        
        if (HealthPlayer.Instance != null && HealthPlayer.Instance.currentHealth <= 0)
            return; 
        isPlayerInside = true;
        selectMarker.SetActive(true);
        chopTimer = 0f;
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
          
            if (HealthPlayer.Instance != null && HealthPlayer.Instance.currentHealth <= 0)
            {
                isPlayerInside = false;
                selectMarker.SetActive(false);
                return;
            }
           

            chopTimer += Time.deltaTime;

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

        
        Vector3 offset = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
        ItemPickupPool.Instance.Get(ItemType.Wood, spawnPos + offset, 1);

        gameObject.SetActive(false);
    }

    
    private void OnEnable()
    {
        selectMarker.SetActive(false);
        isPlayerInside = false;
        chopTimer = 0f;
    }
}