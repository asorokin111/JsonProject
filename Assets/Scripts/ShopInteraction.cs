using UnityEngine;

public class ShopInteraction : MonoBehaviour
{
    public delegate void StoreOpenAction();
    public static event StoreOpenAction OnStoreOpen;
    public delegate void InventoryOpenAction();
    public static event InventoryOpenAction OnInventoryOpen;

    [SerializeField]
    private GameObject _shopMenu;
    [SerializeField]
    private GameObject _inventoryMenu;

    [SerializeField]
    private Collider2D _shopCollider;

    public void OpenShop()
    {
        if (!IsInShop()) return;
        _shopMenu.SetActive(true);
        OnStoreOpen?.Invoke();
    }

    public void OpenInventory()
    {
        _inventoryMenu.SetActive(true);
        OnInventoryOpen?.Invoke();
    }

    private bool IsInShop()
    {
        return _shopCollider.bounds.Contains(transform.position);
    }
}
