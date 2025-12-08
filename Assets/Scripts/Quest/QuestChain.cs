using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Quest Chain", menuName = "Quest System/Quest Chain")]
public class QuestChain : ScriptableObject
{
    public string chainName = "Chuỗi nhiệm vụ chính";
    public List<QuestData> quests = new List<QuestData>();
}