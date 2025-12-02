using UnityEngine;

public class UniversalBuyButton : MonoBehaviour
{
    public void OnBuyClicked()
    {
        // ƯU TIÊN: Nếu đang mở shop ruộng → mua ruộng
        if (FarmPlotShopTrigger.CurrentShop != null)
        {
            FarmPlotShopManager.TryBuyCurrentPlot();
            return;
        }

        // Nếu không phải ruộng → chắc chắn là shop thú → mua thú
        if (ShopTrigger.CurrentShop != null)
        {
            AnimalShopManager.TryBuyCurrentAnimal();
            return;
        }

        Debug.LogWarning("Không có shop nào đang mở để mua!");
    }
}