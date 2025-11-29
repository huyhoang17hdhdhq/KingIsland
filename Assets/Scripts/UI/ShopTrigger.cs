using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private EventInfo shopData;
    [SerializeField] private GameObject interactButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private string customButtonText = "Mở Shop";
    [SerializeField] private Transform animalParent;
    [SerializeField] private Button buyButton;

    private static ShopTrigger currentShop;

    private void Awake()
    {
        AnimalShopManager.Instance.RegisterShop(this);
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

    public EventInfo ShopData => shopData;
    public Transform AnimalParent => animalParent;
    public Button BuyButton => buyButton;
    public static ShopTrigger CurrentShop => currentShop;

    private void OnDestroy()
    {
        if (currentShop == this) currentShop = null;
    }
}