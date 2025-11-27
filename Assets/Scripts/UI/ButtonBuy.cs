using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBuy : MonoBehaviour
{
    public EventInfo info;

    public void OnClick()
    {
        CommonUIPanel.Instance.Show(info);
    }
}
