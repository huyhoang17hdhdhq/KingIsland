using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private EventInfor shopData;
    [SerializeField] private GameObject interactButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private string customButtonText = "Mở Shop";
    [SerializeField] private Transform animalParent;
    [SerializeField] private Button buyButton;

    [SerializeField] private string productionSpeedType = "Cow";

    private static ShopTrigger currentShop;

    
    private void Start()
    {
        if (AnimalShopManager.Instance != null)
        {
            AnimalShopManager.Instance.RegisterShop(this);
        }
        else
        {
            Debug.LogWarning("AnimalShopManager chưa sẵn sàng! Shop này sẽ được đăng ký muộn: " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        currentShop = this;
        interactButton.SetActive(true);
        if (buttonText != null) buttonText.text = customButtonText;
        AnimalShopManager.UpdateCurrentShopUI();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (currentShop == this)
        {
            currentShop = null;
            if (interactButton != null) interactButton.SetActive(false);
        }
    }

    public static void OpenCurrentShop()
    {
        if (currentShop != null && currentShop.shopData != null)
        {
          
            CommonUIPanel.Instance.Show(currentShop.shopData);

            
            AnimalShopManager.UpdateCurrentShopUI();

           
            if (currentShop.interactButton != null)
                currentShop.interactButton.SetActive(false);
        }
    }

    public int GetCurrentAnimalCount()
    {
        if (animalParent == null) return 0;
        return animalParent.childCount; 
    }

    public int GetCurrentLevel()
    {
        return GetCurrentAnimalCount(); 
    }

    public int GetNextLevel()
    {
        return GetCurrentLevel() + 1;
    }



    public string GetProductionSpeedType()
    {
        return productionSpeedType; 
    }

    public EventInfor ShopData => shopData;
    public Transform AnimalParent => animalParent;
    public Button BuyButton => buyButton;
    public static ShopTrigger CurrentShop => currentShop;

    private void OnDestroy()
    {
        if (currentShop == this) currentShop = null;
    }
}