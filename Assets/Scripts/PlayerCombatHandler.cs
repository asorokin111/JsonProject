using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatHandler : MonoBehaviour
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

    private ScreenSide _previousCursorSide = ScreenSide.Left;

    private bool _isAttacking;

    // OnEnable tries to get ActionMap from pre-set InputHandler too early, so I have to
    // create a unique ActionMap in every input script to avoid racing with InputHandler's Awake().
    // The script is huge, so a global ActionMap would be much better.
    private ActionMap _map;

    private InputAction _attackAction;

    enum ScreenSide
    {
        Left,
        Right,
    }

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

    ScreenSide CheckCursorSide()
    {
        return Input.mousePosition.x >= (Screen.width / 2) ? ScreenSide.Right: ScreenSide.Left;
    }

    private void Update()
    {
        if (_isAttacking) return;
        ScreenSide currentCursorSide = CheckCursorSide();
        // Checking if the cursor's screen side changed
        if (currentCursorSide != _previousCursorSide)
        {
            FlipPlayer();
        }
        _previousCursorSide = currentCursorSide;
    }

    private void FlipPlayer()
    {
        Transform playerTransform = transform.parent;
        playerTransform.localScale = new Vector3(
                    -playerTransform.localScale.x,
                    playerTransform.localScale.y,
                    playerTransform.localScale.z
                );
    }

    private void StartAttack(InputAction.CallbackContext context)
    {
        if (_isAttacking) return;
        StartCoroutine(Attack());
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
