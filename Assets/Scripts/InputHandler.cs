using UnityEngine;
using UnityEngine.InputSystem;

// The class that handles movement inputs
public class InputHandler : MonoBehaviour
{
    public ActionMap actionMap;
    public float moveSpeed;
    private Rigidbody2D _rb;
    private Vector2 _moveDirection;

    private InputAction _move;
    private InputAction _fire;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        actionMap = new ActionMap();
    }

    private void OnEnable()
    {
        _move = actionMap.Player.Move;
        _move.Enable();

        _fire = actionMap.Player.Fire;
        _fire.Enable();
        _fire.performed += Fire;

    }

    private void OnDisable()
    {
        _move.Disable();

        _fire.performed -= Fire;
        _fire.Disable();
    }

    private void Update()
    {
        _moveDirection = _move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        
    }
}
