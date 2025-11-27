using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Castle : MonoBehaviour
{
    [Header("Vật phẩm sẽ rơi ra khi Player chạm")]
    [SerializeField] private GameObject rewardPrefab;
    [Header("Cho phép spawn vật phẩm? (true = có, false = không)")]
    [SerializeField] private bool allowReward = true;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected float minChangeTime = 1.5f;
    [SerializeField] protected float maxChangeTime = 4f;

    [Header("=== THANH FILL TIẾN TRÌNH ===")]
    [SerializeField] private Image fillImage;
    [SerializeField] private float fillTime = 10f;

    private Rigidbody2D rb;
    private float timer;
    private bool facingRight = true;
    private float currentFillTime = 0f;
    protected bool isFilling = false;
    

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        ChangeDirection();
    }

    protected virtual void Update()
    {
        if (fillImage != null && !isFilling)
        {
            currentFillTime += Time.deltaTime;
            fillImage.fillAmount = currentFillTime / fillTime;

            if (currentFillTime >= fillTime)
            {
                currentFillTime = fillTime;
                isFilling = true;
                rb.velocity = Vector2.zero; // dừng di chuyển ngay khi đầy
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
        float yRot = facingRight ? 180f : 0f;
        transform.rotation = Quaternion.Euler(0, yRot, 0);
        timer = Random.Range(minChangeTime, maxChangeTime);
    }

    protected virtual void SpawnReward()
    {
        if (rewardPrefab == null || !allowReward) return;
        Instantiate(rewardPrefab, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && allowReward && isFilling)
        {
            SpawnReward();

            currentFillTime = 0f;
            if (fillImage != null)
                fillImage.fillAmount = 0f;

            isFilling = false;
        }
    }
}