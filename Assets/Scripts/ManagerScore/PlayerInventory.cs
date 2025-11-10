using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    [Header("Thời gian nhảy số bằng DOTween")]
    public float tweenDuration = 0.3f;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<TextMeshProUGUI> coinTexts = new();
    public List<TextMeshProUGUI> diamondTexts = new();
    public List<TextMeshProUGUI> lumberTexts = new();
    public List<TextMeshProUGUI> eggTexts = new();
    public List<TextMeshProUGUI> milkTexts = new();

    public int coin;
    public int diamond;
    public int lumber;
    public int egg;
    public int milk;

    private void Start()
    {
        coin = PlayerPrefs.GetInt("Coin", 0);
        diamond = PlayerPrefs.GetInt("Diamond", 0);
        lumber = PlayerPrefs.GetInt("Lumber", 0);
        egg = PlayerPrefs.GetInt("Egg", 0);
        milk = PlayerPrefs.GetInt("Milk", 0);

        UpdateAllUI(true);
    }

    public void AddCoin(int amount) => UpdateValue(ref coin, amount, coinTexts, "Coin");
    public void AddDiamond(int amount) => UpdateValue(ref diamond, amount, diamondTexts, "Diamond");
    public void AddLumber(int amount) => UpdateValue(ref lumber, amount, lumberTexts, "Lumber");
    public void AddEgg(int amount) => UpdateValue(ref egg, amount, eggTexts, "Egg");
    public void AddMilk(int amount) => UpdateValue(ref milk, amount, milkTexts, "Milk");

    public void RemoveCoin(int amount) => UpdateValue(ref coin, -amount, coinTexts, "Coin");
    public void RemoveDiamond(int amount) => UpdateValue(ref diamond, -amount, diamondTexts, "Diamond");
    public void RemoveLumber(int amount) => UpdateValue(ref lumber, -amount, lumberTexts, "Lumber");
    public void RemoveEgg(int amount) => UpdateValue(ref egg, -amount, eggTexts, "Egg");
    public void RemoveMilk(int amount) => UpdateValue(ref milk, -amount, milkTexts, "Milk");


    private void UpdateValue(ref int currentValue, int amount, List<TextMeshProUGUI> texts, string key = "")
    {
        int oldValue = currentValue;
        currentValue = Mathf.Max(0, currentValue + amount);
        AnimateValueChange(oldValue, currentValue, texts);


        if (!string.IsNullOrEmpty(key))
        {
            PlayerPrefs.SetInt(key, currentValue);
            PlayerPrefs.Save();
        }
    }

    private void AnimateValueChange(int from, int to, List<TextMeshProUGUI> texts)
    {
        foreach (var text in texts)
        {
            if (text == null) continue;
            DOTween.Kill(text);
            float value = from;
            DOTween.To(() => value, x =>
            {
                value = x;
                text.text = Mathf.RoundToInt(value).ToString();
            }, to, tweenDuration).SetEase(Ease.OutQuad).SetId(text);
        }
    }


    private void UpdateAllUI(bool instant = false)
    {
        if (instant)
        {
            UpdateUIList(coinTexts, coin);
            UpdateUIList(diamondTexts, diamond);
            UpdateUIList(lumberTexts, lumber);
            UpdateUIList(eggTexts, egg);
            UpdateUIList(milkTexts, milk);
        }
        else
        {
            AnimateValueChange(0, coin, coinTexts);
            AnimateValueChange(0, diamond, diamondTexts);
            AnimateValueChange(0, lumber, lumberTexts);
            AnimateValueChange(0, egg, eggTexts);
            AnimateValueChange(0, milk, milkTexts);
        }
    }

    private void UpdateUIList(List<TextMeshProUGUI> texts, int value)
    {
        foreach (var text in texts)
        {
            if (text != null)
                text.text = value.ToString();
        }
    }
}
