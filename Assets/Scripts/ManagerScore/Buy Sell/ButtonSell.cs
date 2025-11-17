using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSell : MonoBehaviour
{
    public static event Action<GameObject, int> OnButtonClickedEvent;

    private Button button;
    public int slotIndex; 

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickSell);
    }

    private void OnClickSell()
    {
        
        Sellitems.Instance.OpenPanelSell();

        
        OnButtonClickedEvent?.Invoke(this.gameObject, slotIndex);
    }
}
