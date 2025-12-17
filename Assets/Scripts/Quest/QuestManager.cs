using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public QuestChain currentChain;

    private int currentQuestIndex = 0;
    private HashSet<QuestType> completedTypes = new HashSet<QuestType>();
    private HashSet<QuestType> claimedTypes = new HashSet<QuestType>();

    [System.Serializable]
    public class QuestVisualGroup
    {
        public int questIndex;
        public List<GameObject> objects;
    }

    public List<QuestVisualGroup> questVisualGroups = new List<QuestVisualGroup>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        Load();
        UpdateQuestVisuals();
        Debug.Log("QuestManager Awake");



    }

    public void ReportHarvest(ItemType itemType, int amount = 1)
    {
        QuestType questType = itemType switch
        {
            ItemType.Wood => QuestType.HarvestWood,
            ItemType.Egg => QuestType.HarvestEgg,
            ItemType.Filed => QuestType.HarvestWheat,
            ItemType.Milk => QuestType.HarvestMilk,
            _ => QuestType.None
        };
        if (questType != QuestType.None)
            ReportAction(questType, amount);
    }

    public void ReportSellItem(ItemType itemType, int amount = 1)
    {
        QuestType questType = itemType switch
        {
            ItemType.Wood => QuestType.SellWood,
            ItemType.Egg => QuestType.SellEgg,
            _ => QuestType.None
        };
        if (questType != QuestType.None)
            ReportAction(questType, amount);
    }

    public void ReportBuyAnimal(ResourceType animalType, int amount = 1)
    {
        QuestType questType = animalType switch
        {
            ResourceType.Chicken => QuestType.BuyChicken,
            ResourceType.Cow => QuestType.BuyCow,
            _ => QuestType.None
        };
        if (questType != QuestType.None)
            ReportAction(questType, amount);
    }

    public void ReportUpgradeSpeed(string typeKey, int amount = 1)
    {
        QuestType questType = typeKey switch
        {
            "Chicken" => QuestType.UpgradeSpeedChicken,
            "Cow" => QuestType.UpgradeSpeedCow,
            _ => QuestType.None
        };
        if (questType != QuestType.None)
            ReportAction(questType, amount);
    }

    public void ReportUnlockIsland(ResourceType islandType)
    {
        QuestType questType = islandType switch
        {
            ResourceType.UnlockIslandChicken => QuestType.UnlockIslandChicken,
            ResourceType.UnlockIslandCow => QuestType.UnlockIslandCow,
            ResourceType.UnlockIslandFiled => QuestType.UnlockIslandWheat,
            ResourceType.UnlockIslandSugar => QuestType.UnlockIslandSugar,
            _ => QuestType.None
        };
        if (questType != QuestType.None)
            ReportAction(questType, 1);
    }

    public void ReportAction(QuestType type, int amount = 1)
    {
        if (currentChain == null || currentQuestIndex >= currentChain.quests.Count) return;
        if (currentChain.quests[currentQuestIndex].type != type) return;
        if (completedTypes.Contains(type)) return;

        AddProgress(type, amount);
        CheckCompletion();
    }

    private void AddProgress(QuestType type, int amount)
    {
        int current = PlayerPrefs.GetInt("QuestProgress_" + type, 0);
        current += amount;
        PlayerPrefs.SetInt("QuestProgress_" + type, current);
    }

    private void CheckCompletion()
    {
        if (currentChain == null || currentQuestIndex >= currentChain.quests.Count) return;
        var quest = currentChain.quests[currentQuestIndex];
        int progress = PlayerPrefs.GetInt("QuestProgress_" + quest.type, 0);

        if (progress >= quest.requiredAmount && !completedTypes.Contains(quest.type))
        {
            completedTypes.Add(quest.type);
        }
    }

    public void ClaimCurrentReward()
    {
        if (currentChain == null || currentQuestIndex >= currentChain.quests.Count) return;

        var quest = currentChain.quests[currentQuestIndex];
        int progress = PlayerPrefs.GetInt("QuestProgress_" + quest.type, 0);

        if (progress < quest.requiredAmount) return;
        if (claimedTypes.Contains(quest.type)) return;

        ResourceManager.Instance.Add(quest.rewardType, quest.rewardAmount);
        claimedTypes.Add(quest.type);

        currentQuestIndex++;
        Save();
        UpdateQuestVisuals();
    }
    private void UpdateQuestVisuals()
    {
        
    
        foreach (var group in questVisualGroups)
        {
            for (int i = 0; i < group.objects.Count; i++)
            {
                var obj = group.objects[i];
                if (obj == null) continue;

                // CHỈ object index 0 là object quest (bật/tắt theo quest hiện tại)
                if (i == 0)
                {
                    if (group.questIndex == currentQuestIndex)
                        obj.SetActive(true);
                    else
                        obj.SetActive(false);
                }
                else
                {
                    // Các object khác (đảo, world state):
                    // Một khi questIndex <= currentQuestIndex thì chỉ bật, KHÔNG BAO GIỜ TẮT
                    if (!obj.activeSelf && group.questIndex <= currentQuestIndex)
                        obj.SetActive(true);
                }
            }
        }
    }






public QuestData GetCurrentQuest() =>
        currentChain != null && currentQuestIndex < currentChain.quests.Count
        ? currentChain.quests[currentQuestIndex] : null;

    public int GetCurrentProgress()
    {
        var quest = GetCurrentQuest();
        return quest != null ? PlayerPrefs.GetInt("QuestProgress_" + quest.type, 0) : 0;
    }

    public bool IsCurrentQuestCompleted()
    {
        var quest = GetCurrentQuest();
        if (quest == null) return false;
        return GetCurrentProgress() >= quest.requiredAmount;
    }

    public bool IsRewardAlreadyClaimed()
    {
        var quest = GetCurrentQuest();
        return quest != null && claimedTypes.Contains(quest.type);
    }

    private void Save()
    {
        PlayerPrefs.SetInt("ActiveQuestIndex", currentQuestIndex);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        currentQuestIndex = PlayerPrefs.GetInt("ActiveQuestIndex", 0);
    }

    public int GetCurrentQuestNumber() => currentQuestIndex + 1;

    public int GetTotalQuestsInChain() =>
        currentChain != null ? currentChain.quests.Count : 0;

    public string GetQuestProgressText()
    {
        if (currentChain == null) return "Không có nhiệm vụ";

        int current = GetCurrentQuestNumber();
        int total = GetTotalQuestsInChain();

        if (current > total)
            return "HOÀN THÀNH CHUỖI!";

        return $"Quest {current}";
    }
    


}
