using UnityEngine;

public class ButtonBuy : MonoBehaviour
{
    public void OnClick()
    {
        ShopTrigger.OpenCurrentShop();
    }
}