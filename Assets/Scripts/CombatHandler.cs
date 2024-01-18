using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CombatHandler : MonoBehaviour
{
    [Tooltip("The weapon hitbox that will be activated when attacking")]
    [SerializeField]
    private GameObject _hitbox;

    [Header("Combat Settings")]
    [SerializeField]
    private float _windup;
    [SerializeField]
    private float _attackTime;
    [SerializeField]
    private float _cooldown;

    private bool _cursorOnRightSide = false;

    private bool _isAttacking;

    // OnEnable tries to get ActionMap from pre-set InputHandler too early, so I have to
    // create a unique ActionMap in every input script to avoid racing with InputHandler's Awake().
    // The script is huge, so a global ActionMap would be much better.
    private ActionMap _map;

    private InputAction _attackAction;

    private void Awake()
    {
        _map = new ActionMap();
    }

    private void OnEnable()
    {
        _attackAction = _map.Player.Attack;
        _attackAction.Enable();
        _attackAction.performed += StartAttack;
    }

    private void OnDisable()
    {
        _attackAction.performed -= StartAttack;
        _attackAction.Disable();
    }

    private void Update()
    {
        if (!_isAttacking)
        {
            RotateSword();

            // Swapping animation keyframes for it to look mirrored here
            // because RotateSword() only rotates the object and not the animation
            bool previouslyOnRight = _cursorOnRightSide;
            _cursorOnRightSide = Input.mousePosition.x >= Screen.width / 2;
            // Checking if the cursor's screen side changed
            if (_cursorOnRightSide != previouslyOnRight)
            {
                
            }
        }
    }

    private void StartAttack(InputAction.CallbackContext context)
    {
        if (!_isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private void RotateSword()
    {
        Vector2 pos = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = Mathf.Atan2(pos.y - mouse.y, pos.x - mouse.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private IEnumerator Attack()
    {
        _isAttacking = true;
        yield return new WaitForSeconds(_windup);
        _hitbox.SetActive(true);
        yield return new WaitForSeconds(_attackTime);
        _hitbox.SetActive(false);
        yield return new WaitForSeconds(_cooldown);
        _isAttacking = false;
    }
}
