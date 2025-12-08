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

        ResourceType animalType = shop.ShopData.resourceToIncrease; 

        int currentCount = ResourceManager.Instance.Get(animalType);
        int nextPrice = (currentCount + 1) * shop.ShopData.price;
       
        if (ResourceManager.Instance.TrySpend(ResourceType.Gold, nextPrice))
        {
            ResourceManager.Instance.Set(animalType, currentCount + 1);
            GameObject animal = Instantiate(shop.ShopData.prefabToSpawn, shop.AnimalParent);
            animal.transform.localPosition = new Vector3(0, currentCount * 0.15f, 0);
            animal.SetActive(true);

            QuestManager.Instance.ReportBuyAnimal(animalType, 1);

            UpdateCurrentShopUI();
        }
    }

    public static void UpdateCurrentShopUI()
    {
        var shop = ShopTrigger.CurrentShop;
        if (shop == null) return;

        ResourceType animalType = shop.ShopData.resourceToIncrease; 

        int count = ResourceManager.Instance.Get(animalType);
        int nextPrice = (count + 1) * shop.ShopData.price;

        if (shop.BuyButton != null)
            shop.BuyButton.interactable = ResourceManager.Instance.Get(ResourceType.Gold) >= nextPrice;

        if (CommonUIPanel.Instance != null && CommonUIPanel.Instance.gameObject.activeInHierarchy)
        {
            CommonUIPanel.Instance.UpdateLevelText(count, count + 1);
            CommonUIPanel.Instance.UpdatePriceText(nextPrice);
        }
    }

    public static int GetCurrentPrice()
    {
        var shop = ShopTrigger.CurrentShop;
        if (shop == null || shop.ShopData == null) return 0;

        ResourceType animalType = shop.ShopData.resourceToIncrease;

        int count = ResourceManager.Instance.Get(animalType);
        return (count + 1) * shop.ShopData.price;
    }

    private void LoadAndSpawnAnimals(ShopTrigger shop)
    {
        if (shop.ShopData == null || shop.AnimalParent == null) return;

        ResourceType animalType = shop.ShopData.resourceToIncrease;

        int savedCount = ResourceManager.Instance.Get(animalType);

        for (int i = 0; i < savedCount; i++)
        {
            GameObject animal = Instantiate(shop.ShopData.prefabToSpawn, shop.AnimalParent);
            animal.transform.localPosition = new Vector3(0, i * 0.15f, 0);
            animal.SetActive(true);
        }
    }
}