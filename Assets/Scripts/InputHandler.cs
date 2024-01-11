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

    private void Awake()
    {
        actionMap = new ActionMap();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _move = actionMap.Player.Move;
        _move.Enable();
    }

    private void OnDisable()
    {
        _move.Disable();
    }

    private void Update()
    {
        _moveDirection = _move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
    }
}
