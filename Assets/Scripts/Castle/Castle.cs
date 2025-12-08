using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Castle : MonoBehaviour
{
    [Header("Cho phép spawn vật phẩm?")]
    [SerializeField] private bool allowReward = true;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected float minChangeTime = 1.5f;
    [SerializeField] protected float maxChangeTime = 4f;

    [Header("=== THANH FILL TIẾN TRÌNH ===")]
    [SerializeField] private Image fillImage;
    [SerializeField] protected float fillTime = 10f;


    private Rigidbody2D rb;
    private float timer;
    private bool facingRight = true;
    private float currentFillTime = 0f;
    protected bool isFilling = false;

    // ĐÃ THÊM DÒNG NÀY – CHÍNH LÀ CÁI GÂY LỖI!
    private string speedTypeKey = "Unknown"; // Sẽ được gán trong Start()

    protected virtual ItemType RewardType => ItemType.Egg;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        // LẤY KEY TỐC ĐỘ CHỈ 1 LẦN DUY NHẤT – DÙNG CHUNG CHO RAU + ĐỘNG VẬT
        var shop = GetComponentInParent<ShopTrigger>();
        if (shop != null)
        {
            speedTypeKey = shop.GetProductionSpeedType(); // Dùng field trong ShopTrigger
        }

        ChangeDirection();
    }

    protected virtual void Update()
    {
        if (fillImage != null && !isFilling)
        {
            currentFillTime += Time.deltaTime;

            // DÙNG speedTypeKey ĐÃ LƯU – KHÔNG GỌI GetComponentInParent NỮA!
            float actualFillTime = ProductionSpeedManager.Instance.GetActualFillTime(fillTime, speedTypeKey);

            fillImage.fillAmount = currentFillTime / actualFillTime;

            if (currentFillTime >= actualFillTime)
            {
                currentFillTime = actualFillTime;
                isFilling = true;
                rb.velocity = Vector2.zero;
            }
            else
            {
                Move();
            }
        }
    }

    public void Move()
    {
        rb.velocity = -transform.right * speed;
        timer -= Time.deltaTime;
        if (timer <= 0f)
            ChangeDirection();
    }

    protected virtual void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 180f : 0f, 0);
        timer = Random.Range(minChangeTime, maxChangeTime);
    }

    protected virtual void SpawnReward()
    {
        if (!allowReward) return;

        ItemPickupPool.Instance.Get(
            type: RewardType,
            position: transform.position + Vector3.up * 0.05f,
            amount: 1
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && allowReward && isFilling)
        {
            SpawnReward();
            currentFillTime = 0f;
            if (fillImage != null) fillImage.fillAmount = 0f;
            isFilling = false;
        }
    }
}