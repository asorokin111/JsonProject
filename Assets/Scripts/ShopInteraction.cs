using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteraction : MonoBehaviour
{
    [SerializeField]
    private GameObject _shopMenu;
    [SerializeField]
    // The object with the shop script
    private GameObject _shop;

    public void OpenShop()
    {
        _shop.SetActive(true);
        _shopMenu.SetActive(true);
    }

    public void CloseShop()
    {
        _shopMenu.SetActive(false);
        _shop.SetActive(false);
    }
}
