using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthPlayer : MonoBehaviour
{
    [Header("=== CÀI ĐẶT MÁU NGƯỜI CHƠI ===")]
    [SerializeField] private int baseMaxHealth = 5; // Máu cơ bản
    [SerializeField] private ResourceType extraMaxHealthKey = ResourceType.ExtraMaxHealth;

    [SerializeField] private Image healthFillImage;
    [SerializeField] private TextMeshProUGUI healthText;

    public int currentHealth;
    public static HealthPlayer Instance { get; private set; }

    // MÁU TỐI ĐA HIỆN TẠI = CƠ BẢN + EXTRA
    public int MaxHealth => baseMaxHealth + ResourceManager.Instance.Get(extraMaxHealthKey);

    private void Awake()
    {
        Instance = this;
        currentHealth = MaxHealth;
        UpdateHealthUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Castle"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return;
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(0, currentHealth);
        UpdateHealthUI();
        Debug.Log($"PLAYER MẤT MÁU! Còn lại: {currentHealth}/{MaxHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void UpdateHealthUI()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = (float)currentHealth / MaxHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{MaxHealth}";
        }
    }

    private void Die()
    {
        Debug.Log("PLAYER CHẾT RỒI ĐẠI CA ƠI!!!");
    }

    [ContextMenu("Test Mất 1 Máu")]
    private void TestTakeDamage()
    {
        TakeDamage(1);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, MaxHealth);
        UpdateHealthUI();
    }
}