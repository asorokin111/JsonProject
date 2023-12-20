using UnityEngine;

public class ShopInteraction : MonoBehaviour
{
    public delegate void StoreOpenAction();
    public static event StoreOpenAction OnStoreOpen;

    [SerializeField]
    private GameObject _shopMenu;
    [SerializeField]
    private Collider2D _shopCollider;

    public void OpenShop()
    {
        if (!IsInShop()) return;
        _shopMenu.SetActive(true);
        OnStoreOpen?.Invoke();
    }

    public void CloseShop()
    {
        _shopMenu.SetActive(false);
    }

    private bool IsInShop()
    {
        return _shopCollider.bounds.Contains(transform.position);
    }
}
