using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class CropGrowController : MonoBehaviour
{
    [Header("=== THỜI GIAN MỌC 1 LƯỢT (giây) ===")]
    [SerializeField] protected float growTime = 30f;

    [Header("=== THANH TIẾN ĐỘ (LUÔN HIỆN & CHẠY FILL) ===")]
    [SerializeField] protected Image progressFill;

    [Header("=== PHẦN CÂY RAU (ẨN KHI ĐANG MỌC) ===")]
    [SerializeField] protected GameObject cropVisual;

    private Coroutine growRoutine;
    private Collider2D col;


    protected virtual ItemType RewardType => ItemType.Filed;

    protected virtual void Start()
    {
        col = GetComponent<Collider2D>(); 
        StartGrowing();
    }
    protected virtual void StartGrowing()
    {
        if (cropVisual != null)
            cropVisual.SetActive(false);

        if (progressFill != null)
        {
            progressFill.gameObject.SetActive(true);
            progressFill.fillAmount = 0f;
        }

        if (growRoutine != null) StopCoroutine(growRoutine);
        growRoutine = StartCoroutine(GrowRoutine());
    }

    private IEnumerator GrowRoutine()
    {
        float elapsed = 0f;
        while (elapsed < growTime)
        {
            elapsed += Time.unscaledDeltaTime;
            if (progressFill != null)
                progressFill.fillAmount = elapsed / growTime;

            if (col != null)
            {
                col.enabled = (elapsed >= growTime);
            }
          
            yield return null;
        }
        
        FinishGrowing();
    }

    protected virtual void FinishGrowing()
    {
        if (cropVisual != null)
            cropVisual.SetActive(true);

        if (progressFill != null)
            progressFill.fillAmount = 1f;

        growRoutine = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (growRoutine != null) return;

        if (HealthPlayer.Instance != null && HealthPlayer.Instance.currentHealth <= 0)
            return;

        Harvest();
    }

    protected virtual void Harvest()
    {
        ItemPickupPool.Instance.Get(
            type: RewardType,
            position: transform.position,
            amount: 1
        );

        StartGrowing();
    }

    public virtual void InstantFinish()
    {
        if (growRoutine != null) StopCoroutine(growRoutine);
        FinishGrowing();
    }

    public virtual void SpeedUpPercent(float percent)
    {
        if (growRoutine == null) return;

        float currentProgress = progressFill != null ? progressFill.fillAmount : 0f;
        growTime = Mathf.Max(1f, growTime * (1f - percent / 100f));
        StartGrowing();
        if (progressFill != null)
            progressFill.fillAmount = currentProgress;
    }
}