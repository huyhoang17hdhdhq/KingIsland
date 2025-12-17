using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyRewards : MonoBehaviour
{
    [Header("=== 7 NGÀY PHẦN THƯỞNG ===")]
    [SerializeField] private DailyRewardSlot[] rewardSlots;

    private const string LAST_CLAIM_DATE_KEY = "DailyReward_LastClaimDate";
    private const string LAST_CLAIM_DAY_KEY = "DailyReward_LastClaimDay";




    private void Start()
    {
        UpdateAllSlots();
    }

    private void UpdateAllSlots()
    {
        int lastClaimedDay = PlayerPrefs.GetInt(LAST_CLAIM_DAY_KEY, 0);

        for (int i = 0; i < rewardSlots.Length; i++)
        {
            int dayIndex = i + 1;
            bool isClaimed = lastClaimedDay >= dayIndex;
            bool canClaimToday = !isClaimed && lastClaimedDay == i &&
            IsNewDay();





            rewardSlots[i].Setup(dayIndex, isClaimed, canClaimToday, ClaimReward);
        }
    }

    private void ClaimReward(int dayIndex)
    {
        var slot = rewardSlots[dayIndex - 1];

        // + TẤT CẢ PHẦN THƯỞNG TRONG LIST
        foreach (var reward in slot.rewards)
        {
            if (reward.amount <= 0) continue;

            if (reward.source == RewardSource.ResourceManager)
            {
                ResourceManager.Instance.Add(reward.resourceType, reward.amount);
            }
            else // PlayerInventory
            {
                PlayerInventory.Instance.AddItem(reward.itemType, reward.amount);
            }
        }

        PlayerPrefs.SetInt(LAST_CLAIM_DAY_KEY, dayIndex);
        PlayerPrefs.Save();
        PlayerPrefs.SetString(LAST_CLAIM_DATE_KEY, DateTime.Now.ToString());



        PlayerPrefs.Save();


        slot.MarkAsClaimed();
    }
    private bool IsNewDay()
    {
        if (!PlayerPrefs.HasKey(LAST_CLAIM_DATE_KEY))
            return true;

        DateTime lastDate = DateTime.Parse(
            PlayerPrefs.GetString(LAST_CLAIM_DATE_KEY)
        );

        return DateTime.Now.Date > lastDate.Date;
    }


    [System.Serializable]
    public class DailyRewardSlot
    {
       
        public Button claimButton;
        public GameObject checkMark;
        public GameObject lockOverlay;

        [Header("=== NHIỀU PHẦN THƯỞNG CÙNG LÚC ===")]
        public List<RewardItem> rewards = new List<RewardItem>();

        private Action<int> onClaimCallback;
        private int dayIndex;

        public void Setup(int day, bool claimed, bool canClaim, Action<int> callback)
        {
            dayIndex = day;
            onClaimCallback = callback;

            

            claimButton.interactable = canClaim;
            checkMark.SetActive(claimed);
            lockOverlay.SetActive(!canClaim && !claimed);

            claimButton.onClick.RemoveAllListeners();
            if (canClaim)
                claimButton.onClick.AddListener(() => onClaimCallback?.Invoke(day));
        }

        public void MarkAsClaimed()
        {
            claimButton.interactable = false;
            checkMark.SetActive(true);
            lockOverlay.SetActive(false);
        }
    }

    [System.Serializable]
    public class RewardItem
    {
        public RewardSource source;
        public ItemType itemType;
        public ResourceType resourceType;
        public int amount = 100;
    }

    public enum RewardSource
    {
        ResourceManager,
        PlayerInventory
    }
}