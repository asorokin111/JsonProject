using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D _rb;
    private Vector2 _moveDirection;

    private ActionMap _actionMap;
    private InputAction _move;
    private InputAction _fire;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _actionMap = new ActionMap();
    }

    private void OnEnable()
    {
        _move = _actionMap.Player.Move;
        _move.Enable();

        _fire = _actionMap.Player.Fire;
        _fire.Enable();
        _fire.performed += Fire;
    }

    private void OnDisable()
    {
        _move.Disable();
        _fire.Disable();
        _fire.performed -= Fire;
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
