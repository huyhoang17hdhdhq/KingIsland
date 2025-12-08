using UnityEngine;

public class UniversalBuyButton : MonoBehaviour
{
    public void OnBuyClicked()
    {
        
        if (FarmPlotShopTrigger.CurrentShop != null)
        {
            FarmPlotShopManager.TryBuyCurrentPlot();
            return;
        }

       
        if (ShopTrigger.CurrentShop != null)
        {
            AnimalShopManager.TryBuyCurrentAnimal();
            return;
        }

        Debug.LogWarning("Không có shop nào đang mở để mua!");
    }
}