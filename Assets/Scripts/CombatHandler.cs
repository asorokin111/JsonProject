using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Swing animation settings")]
    [SerializeField]
    private AnimationClip _swingAnimation;

    [Header("Left side animation")]
    // I have to specify every variable here by itself because keyframes can't be serialized
    [SerializeField]
    private float _leftAnimStart = 0.0f;
    [SerializeField]
    private float _leftAnimEnd = 1.0f;
    [SerializeField]
    private float _leftAnimStartRotation = 30.0f;
    [SerializeField]
    private float _leftAnimEndRotation = 130.0f;

    [Header("Right side animation")]
    [SerializeField]
    private float _rightAnimStart = 0.0f;
    [SerializeField]
    private float _rightAnimEnd = 1.0f;
    [SerializeField]
    private float _rightAnimStartRotation = -30.0f;
    [SerializeField]
    private float _rightAnimEndRotation = -130.0f;

    private Keyframe[] _leftKeys;
    private Keyframe[] _rightKeys;
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

        _leftKeys = new Keyframe[2];
        _leftKeys[0] = new Keyframe(_leftAnimStart, _leftAnimStartRotation);
        _leftKeys[1] = new Keyframe(_leftAnimEnd, _leftAnimEndRotation);

        _rightKeys = new Keyframe[2];
        _rightKeys[0] = new Keyframe(_rightAnimStart, _rightAnimStartRotation);
        _rightKeys[1] = new Keyframe(_rightAnimEnd, _rightAnimEndRotation);
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
                Debug.Log(true);
                var curve = new AnimationCurve(_cursorOnRightSide ? _rightKeys : _leftKeys);
                _swingAnimation.SetCurve("SwordHitbox", typeof(Transform), "localRotation.z", curve);
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
