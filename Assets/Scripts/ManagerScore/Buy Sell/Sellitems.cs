using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sellitems : MonoBehaviour
{
    public static Sellitems Instance;
    public GameObject PanelSell;
    public GameObject Informationitems;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    public void OpenPanelSell()
    {
        PanelSell.SetActive(true);
        Informationitems.SetActive(true);

    }
    public void ClosePanelSell()
    {
        PanelSell.SetActive(false);
        Informationitems.SetActive(false);
    }

   
}
