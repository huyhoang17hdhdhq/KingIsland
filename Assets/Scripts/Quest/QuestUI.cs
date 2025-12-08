using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [Header("=== TEXT HIỆN NHIỆM VỤ ===")]
    public TextMeshProUGUI titleText;
   
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI questOrderText;
    public Button claimButton;

    private void Update()
    {
        UpdateQuestDisplay();
        if (questOrderText != null)
            questOrderText.text = QuestManager.Instance.GetQuestProgressText();
    }

    public void UpdateQuestDisplay()
    {
        var quest = QuestManager.Instance.GetCurrentQuest();
        if (quest == null)
        {
            titleText.text = "Không có nhiệm vụ";
            
            progressText.text = "";
            rewardText.text = "";
            claimButton.gameObject.SetActive(false);
            return;
        }

        // HIỆN TIÊU ĐỀ
        titleText.text = quest.title;

      
       

        // HIỆN TIẾN ĐỘ
        int current = QuestManager.Instance.GetCurrentProgress();
        progressText.text = $"{current}/{quest.requiredAmount}";

        // HIỆN THƯỞNG
        rewardText.text = $"+{quest.rewardAmount}";

        // NÚT NHẬN THƯỞNG (nếu đã hoàn thành)
        bool isDone = current >= quest.requiredAmount;
        claimButton.gameObject.SetActive(isDone);
        claimButton.interactable = isDone;
    }

    public void OnClaimButton()
    {
        QuestManager.Instance.ClaimCurrentReward();
        UpdateQuestDisplay();
    }
}