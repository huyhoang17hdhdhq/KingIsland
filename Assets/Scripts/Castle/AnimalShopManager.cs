using UnityEngine;

public class AnimalShopManager : MonoBehaviour
{
    public static AnimalShopManager Instance { get; private set; }

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

    public void RegisterShop(ShopTrigger shop)
    {
        LoadAndSpawnAnimals(shop);
    }

    
    public static void TryBuyCurrentAnimal()
    {
        var shop = ShopTrigger.CurrentShop;
        if (shop == null || shop.ShopData == null) return;

        string key = "BoughtCount_" + shop.ShopData.name;
        int count = PlayerPrefs.GetInt(key, 0);
        int nextPrice = (count + 1) * shop.ShopData.price;

        if (GoldManager.Instance.GetGold() >= nextPrice)
        {
            GoldManager.Instance.RemoveGold(nextPrice);

            count++;
            PlayerPrefs.SetInt(key, count);
            PlayerPrefs.Save();

            GameObject animal = Instantiate(shop.ShopData.prefabToSpawn, shop.AnimalParent);
            animal.transform.localPosition = new Vector3(0, (count - 1) * 0.15f, 0);
            animal.SetActive(true);

            UpdateCurrentShopUI();
        }
    }

    public static void UpdateCurrentShopUI()
    {
        var shop = ShopTrigger.CurrentShop;
        if (shop == null) return;

     
        if (shop.BuyButton != null)
        {
            string key = "BoughtCount_" + shop.ShopData.name;
            int count = PlayerPrefs.GetInt(key, 0);
            int nextPrice = (count + 1) * shop.ShopData.price;
            bool canAfford = GoldManager.Instance.GetGold() >= nextPrice;
            shop.BuyButton.interactable = canAfford;
        }

        
        if (CommonUIPanel.Instance != null && CommonUIPanel.Instance.gameObject.activeInHierarchy)
        {
            int currentLevel = shop.GetCurrentLevel();
            CommonUIPanel.Instance.UpdateLevelText(currentLevel, currentLevel + 1);
        }
    }

    public static int GetCurrentPrice()
    {
        var shop = ShopTrigger.CurrentShop;
        if (shop == null || shop.ShopData == null) return 0;

        string key = "BoughtCount_" + shop.ShopData.name;
        int count = PlayerPrefs.GetInt(key, 0);
        return (count + 1) * shop.ShopData.price;
    }

    private void LoadAndSpawnAnimals(ShopTrigger shop)
    {
        if (shop.ShopData == null || shop.AnimalParent == null) return;

        string key = "BoughtCount_" + shop.ShopData.name;
        int savedCount = PlayerPrefs.GetInt(key, 0);

        for (int i = 0; i < savedCount; i++)
        {
            GameObject animal = Instantiate(shop.ShopData.prefabToSpawn, shop.AnimalParent);
            animal.transform.localPosition = new Vector3(0, i * 0.15f, 0);
            animal.SetActive(true);
        }
    }
}