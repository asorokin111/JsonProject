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
    private int _damage;
    [SerializeField]
    private float _windup;
    [SerializeField]
    private float _attackTime;
    [SerializeField]
    private float _cooldown;

    private bool _isAttacking;

    // OnEnable tries to get ActionMap from pre-set InputHandler too early, so I have to
    // create a unique ActionMap in every input script to avoid racing with InputHandler's Awake()
    // the script is huge, so a global ActionMap would be much better
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

    private void StartAttack(InputAction.CallbackContext context)
    {
        if (!_isAttacking)
        {
            StartCoroutine(Attack());
        }
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
