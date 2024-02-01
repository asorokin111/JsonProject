using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatHandler : CombatHandler
{
    private ScreenSide _previousCursorSide = ScreenSide.Left;

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
        _attackAction.performed += (InputAction.CallbackContext context) => StartAttack();
    }

    private void OnDisable()
    {
        _attackAction.performed -= (InputAction.CallbackContext context) => StartAttack();
        _attackAction.Disable();
    }

    protected override ScreenSide CheckTargetPosition()
    {
        return Input.mousePosition.x >= (Screen.width / 2) ? ScreenSide.Right: ScreenSide.Left;
    }

    private void Update()
    {
        if (_isAttacking) return;
        ScreenSide currentCursorSide = CheckTargetPosition();
        // Checking if the cursor's screen side changed
        if (currentCursorSide != _previousCursorSide)
        {
            OrientateTowardsTarget();
        }
        _previousCursorSide = currentCursorSide;
    }

    protected override void OrientateTowardsTarget()
    {
        Transform playerTransform = transform.parent;
        playerTransform.localScale = new Vector3(
                    -playerTransform.localScale.x,
                    playerTransform.localScale.y,
                    playerTransform.localScale.z
                );
    }
}
