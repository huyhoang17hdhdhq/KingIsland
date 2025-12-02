using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CropGrowController : MonoBehaviour
{
    [Header("=== THỜI GIAN MỌC 1 LƯỢT (giây) ===")]
    [SerializeField] private float growTime = 30f;

    [Header("=== THANH TIẾN ĐỘ (LUÔN HIỆN & CHẠY FILL) ===")]
    [SerializeField] private Image progressFill;           // KÉO IMAGE FILL VÀO ĐÂY

    [Header("=== PHẦN CÂY RAU (ẨN KHI ĐANG MỌC) ===")]
    [SerializeField] private GameObject cropVisual;         // Kéo sprite cây vào (có thể để trống = ẩn cả gốc)

    private Coroutine growRoutine;

    private void Start()
    {
        // Bắt đầu mọc ngay khi vào game / spawn cây
        StartGrowing();
    }

    private void StartGrowing()
    {
        // Ẩn cây (chưa mọc xong)
        if (cropVisual != null)
            cropVisual.SetActive(false);

        // Thanh tiến độ luôn hiện và bắt đầu từ 0
        if (progressFill != null)
        {
            progressFill.gameObject.SetActive(true);
            progressFill.fillAmount = 0f;
        }

        // Bắt đầu đếm
        if (growRoutine != null) StopCoroutine(growRoutine);
        growRoutine = StartCoroutine(GrowRoutine());
    }

    private IEnumerator GrowRoutine()
    {
        float elapsed = 0f;

        while (elapsed < growTime)
        {
            elapsed += Time.unscaledDeltaTime;
            float ratio = elapsed / growTime;

            // Thanh fill chạy từ 0 → 1 theo thời gian thực
            if (progressFill != null)
                progressFill.fillAmount = ratio;

            yield return null;
        }

        // Xong → mọc cây
        FinishGrowing();
    }

    private void FinishGrowing()
    {
        if (cropVisual != null)
            cropVisual.SetActive(true);

        if (progressFill != null)
            progressFill.fillAmount = 1f; // chắc chắn đầy 100%

        growRoutine = null;
    }

    // Player chạm vào cây khi đã mọc xong → Thu hoạch
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (growRoutine != null) return; // chưa mọc xong thì không cho thu hoạch

        Harvest();
    }

    private void Harvest()
    {
        // === Ở ĐÂY MÀY THÊM TIỀN, VẬT PHẨM, ÂM THANH... ===
        // Ví dụ:
        // ResourceManager.Instance.Add(ResourceType.Gold, 10);
        // AudioManager.Play("harvest");

        // Thu hoạch xong → bắt đầu chu kỳ mới
        StartGrowing();
    }

    // ==============================================
    // DÙNG PHÂN BÓN / KIM CƯƠNG TĂNG TỐC
    // ==============================================
    public void InstantFinish() // mọc ngay lập tức
    {
        if (growRoutine != null) StopCoroutine(growRoutine);
        FinishGrowing();
    }

    public void SpeedUpPercent(float percent) // SpeedUpPercent(50) = nhanh hơn 50%
    {
        if (growRoutine == null) return;

        float elapsed = growTime - (progressFill.fillAmount * growTime);
        growTime = growTime * (1f - percent / 100f);
        growTime = Mathf.Max(1f, growTime);

        // Restart lại với thời gian mới
        StartGrowing();
        // Giữ lại tiến độ cũ
        float newElapsed = elapsed * (growTime / (growTime + elapsed));
        progressFill.fillAmount = 1f - (newElapsed / growTime);
    }
}