using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject bag;
    public GameObject setting;
    public GameObject daily;
    public GameObject option;
    public GameObject Sell;

    private void Start()
    {
        bag.SetActive(false);
        setting.SetActive(false);
        daily.SetActive(false);
    }

    public void OpenBag()
    {
       bag.SetActive(true);
        option.SetActive(false);
    }
        
    public void OpenSetting()
    {
        setting.SetActive(true);
        option.SetActive(false);
    }
    public void OpenSell() => Sell.SetActive(true);

    public void OpenDaily()
    {
        daily.SetActive(true);
        option.SetActive(false);
    }

    public void Close()
    {
        bag.SetActive(false);
        setting.SetActive(false);
        daily.SetActive(false);
        option.SetActive(true);
        Sell.SetActive(false);
    }





}
