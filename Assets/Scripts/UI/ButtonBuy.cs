using UnityEngine;
public class ButtonBuy : MonoBehaviour
{
    public void OnClick()
    {
        
        FarmPlotShopTrigger.OpenCurrentShop();
        ShopTrigger.OpenCurrentShop();
       
    }
}