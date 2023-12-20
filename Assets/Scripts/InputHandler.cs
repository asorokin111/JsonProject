using UnityEngine;
using UnityEngine.InputSystem;

// The class that handles movement inputs
public class InputHandler : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D _rb;
    private ShopInteraction _shopInteraction;
    private Vector2 _moveDirection;

    private ActionMap _actionMap;
    private InputAction _move;
    private InputAction _fire;
    private InputAction _interact;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _shopInteraction = GetComponent<ShopInteraction>();
        _actionMap = new ActionMap();
    }

    private void OnEnable()
    {
        _move = _actionMap.Player.Move;
        _move.Enable();

        _fire = _actionMap.Player.Fire;
        _fire.Enable();
        _fire.performed += Fire;

        _interact = _actionMap.Player.Interact;
        _interact.Enable();
        _interact.performed += InteractWithTrader;
    }

    private void OnDisable()
    {
        _move.Disable();

        _fire.performed -= Fire;
        _fire.Disable();

        _interact.performed -= InteractWithTrader;
        _interact.Disable();
    }

    private void Update()
    {
        _moveDirection = _move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
    }

    private void InteractWithTrader(InputAction.CallbackContext context)
    {
        _shopInteraction.OpenShop();
    }

    private void Fire(InputAction.CallbackContext context)
    {
        
    }
}
