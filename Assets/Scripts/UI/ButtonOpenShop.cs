using UnityEngine;
public class ButtonOpenShop : MonoBehaviour
{
    public void OnClick()
    {
       
       
        ShopTrigger.OpenCurrentShop();
        FarmPlotShopTrigger.OpenCurrentShop();

    }
}