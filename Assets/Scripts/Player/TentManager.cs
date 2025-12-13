using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TentManager : MonoBehaviour
{
    [Header("=== CÀI ĐẶT LỀU HỒI MÁU ===")]
    [SerializeField] private GameObject healButton;                    // Nút hồi máu
    [SerializeField] private List<GameObject> objectsToHide;          // List object ẩn khi hồi máu
    [SerializeField] private float healSpeed = 5f;                     // Máu hồi mỗi giây (mày chỉnh thoải mái)
    [SerializeField] private GameObject healFX;                        // Hiệu ứng hồi máu

    [Header("=== LƯU +1 MÁU TỐI ĐA KHI HỒI TỪ 0 ===")]
    [SerializeField] private ResourceType extraMaxHealthKey = ResourceType.ExtraMaxHealth;

    private bool isHealing = false;
    private Button btn;

    private void Awake()
    {
        if (healButton != null)
        {
            btn = healButton.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(StartHealing);
            healButton.SetActive(false);
        }
        if (healFX != null)
            healFX.SetActive(false);
    }

    private void OnDestroy()
    {
        if (btn != null)
            btn.onClick.RemoveListener(StartHealing);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isHealing)
        {
            healButton.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && healButton != null)
        {
            healButton.SetActive(false);
        }
    }

    private void StartHealing()
    {
        if (isHealing || HealthPlayer.Instance == null) return;
        healButton.SetActive(false);
        StartCoroutine(HealingCoroutine());
    }

    private IEnumerator HealingCoroutine()
    {
        isHealing = true;

        foreach (var obj in objectsToHide)
            if (obj != null) obj.SetActive(false);

        if (healFX != null)
            healFX.SetActive(true);

        MusicManager.Instance.HealthSound();

        bool wasDead = HealthPlayer.Instance.currentHealth <= 0;

        // HỒI ĐẾN MÁU TỐI ĐA HIỆN TẠI
        float accumulatedHeal = 0f;
        while (HealthPlayer.Instance.currentHealth < HealthPlayer.Instance.MaxHealth)
        {
            accumulatedHeal += healSpeed * Time.deltaTime;
            int healThisFrame = Mathf.FloorToInt(accumulatedHeal);
            if (healThisFrame > 0)
            {
                HealthPlayer.Instance.Heal(healThisFrame);
                accumulatedHeal -= healThisFrame;
            }
            yield return null;
        }

        // SAU KHI ĐÃ ĐẦY → MỚI +1 MÁU TỐI ĐA → SAU ĐÓ HỒI NỐT 1 MÁU!!!
        if (wasDead)
        {
            int currentExtra = ResourceManager.Instance.Get(extraMaxHealthKey);
            ResourceManager.Instance.Set(extraMaxHealthKey, currentExtra + 1);

            // HỒI THÊM 1 MÁU ĐỂ ĐẦY MỚI!!!
            HealthPlayer.Instance.Heal(1);

            Debug.Log($"+1 MÁU TỐI ĐA! Đã hồi thêm 1 máu để đầy!");
        }

        foreach (var obj in objectsToHide)
            if (obj != null) obj.SetActive(true);

        if (healFX != null)
            healFX.SetActive(false);

        isHealing = false;
    }
}