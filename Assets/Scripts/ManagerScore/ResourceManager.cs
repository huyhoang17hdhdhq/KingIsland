using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ResourceType
{
    Gold,
    Diamond,
    Gem,
    Wood,
    Food,
    Star,
    Mana,
    Ticket,
 
}

[Serializable]
public class ResourceData
{
    public ResourceType type;
    public int amount;
    public List<TextMeshProUGUI> displayTexts;
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [SerializeField] private List<ResourceData> resources = new List<ResourceData>();
    private Dictionary<ResourceType, ResourceData> resourceDict = new Dictionary<ResourceType, ResourceData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

       
        foreach (var res in resources)
        {
            resourceDict[res.type] = res;
            
            int saved = PlayerPrefs.GetInt("Resource_" + res.type, res.amount);
            res.amount = saved;
            UpdateUI(res.type);
        }
    }

    public int Get(ResourceType type) => resourceDict[type].amount;

    public void Add(ResourceType type, int amount)
    {
        var res = resourceDict[type];
        res.amount += amount;
        Save(res.type);
        UpdateUI(res.type);
    }

    public bool TrySpend(ResourceType type, int amount)
    {
        if (Get(type) >= amount)
        {
            resourceDict[type].amount -= amount;
            Save(type);
            UpdateUI(type);
            return true;
        }
        return false;
    }

    private void Save(ResourceType type)
    {
        PlayerPrefs.SetInt("Resource_" + type, resourceDict[type].amount);
    }

    private void UpdateUI(ResourceType type)
    {
        var res = resourceDict[type];
        foreach (var txt in res.displayTexts)
        {
            if (txt != null)
                txt.text = res.amount.ToString();
        }
    }
}