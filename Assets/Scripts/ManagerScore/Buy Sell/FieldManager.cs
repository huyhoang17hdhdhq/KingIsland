//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class FieldManager : MonoBehaviour
//{
//    [Header("Button mở field")]
//    public Button unlockButton;
//    [Header("Giá vàng để mở mỗi ô")]
//    public int unlockPrice = 50;
//    private List<GameObject> childFields = new List<GameObject>();
//    private int nextIndex = 0;
//    private void Start()
//    {
//        childFields.Clear();

//        for (int i = 0; i < transform.childCount; i++)
//        {
//            GameObject child = transform.GetChild(i).gameObject;
//            childFields.Add(child);
//        }

//        LoadFieldsState();

//        if (unlockButton != null)
//            unlockButton.onClick.AddListener(TryUnlockNext);
//    }
//    private void TryUnlockNext()
//    {

//        if (nextIndex >= childFields.Count)
//        {
//            Debug.Log("Tất cả field đã được mở!");
//            return;
//        }
//        int gold = ResourceManager.Instance.Get(ResourceType.Gold);
//        if (gold < unlockPrice)
//        {
//            Debug.Log("Không đủ vàng để mở! Cần: " + unlockPrice);
//            return;
//        }

//        ResourceManager.Instance.TrySpend(ResourceType.Gold, unlockPrice);

//        GameObject field = childFields[nextIndex];
//        field.SetActive(true);

//        PlayerPrefs.SetInt(GetFieldKey(nextIndex), 1);
//        PlayerPrefs.Save();
//        Debug.Log("Đã mở field index " + nextIndex);
//        nextIndex++;
//    }
//    private void LoadFieldsState()
//    {
//        nextIndex = 0;
//        for (int i = 0; i < childFields.Count; i++)
//        {
//            int unlocked = PlayerPrefs.GetInt(GetFieldKey(i), 0);

//            if (unlocked == 1)
//            {
//                childFields[i].SetActive(true);
//                nextIndex = i + 1;
//            }
//            else
//            {
//                childFields[i].SetActive(false);
//            }
//        }
//    }

//    private string GetFieldKey(int index)
//    {
//        return "Field_" + index;
//    }
//}