using UnityEngine;
using UnityEngine.InputSystem;

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

    // Input system
    private ActionMap _map;
    private InputAction _interact;

    private void Awake()
    {
        _map = new ActionMap();
    }

    private void OnEnable()
    {
        _interact = _map.Player.Interact;
        _interact.Enable();
        _interact.performed += OpenShop;
    }

    private void OnDisable()
    {
        _interact.performed -= OpenShop;
        _interact.Disable();
    }

    public void OpenShop(InputAction.CallbackContext context)
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
