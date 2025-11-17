using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    [Header("Danh sách Text hiển thị vàng")]
    public List<TextMeshProUGUI> goldTexts;

    [Header("Số vàng khởi tạo")]
    public int startingGold = 0;

    private int currentGold;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 

       
        currentGold = PlayerPrefs.GetInt("Gold", startingGold);
        UpdateGoldUI();
    }

    public int GetGold()
    {
        return currentGold;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        SaveGold();
        UpdateGoldUI();
    }

    public void RemoveGold(int amount)
    {
        currentGold = Mathf.Max(0, currentGold - amount);
        SaveGold();
        UpdateGoldUI();
    }

    private void SaveGold()
    {
        PlayerPrefs.SetInt("Gold", currentGold);
        PlayerPrefs.Save();
    }

    private void UpdateGoldUI()
    {
        foreach (var txt in goldTexts)
        {
            if (txt != null)
                txt.text = currentGold.ToString();
        }
    }
}
