using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public class QuestData : ScriptableObject
{
    public string title = "Thu hoạch 10 gỗ";
  

    public QuestType type;
    public int requiredAmount = 10;

    public ResourceType rewardType = ResourceType.Diamond;
    public int rewardAmount = 5;
}