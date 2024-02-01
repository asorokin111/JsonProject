using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatHandler : MonoBehaviour
{
    [SerializeField]
    protected GameObject _weaponHitbox;
    [Header("Combat Settings")]
    [SerializeField]
    protected float _windup;
    [SerializeField]
    protected float _attackTime;
    [SerializeField]
    protected float _cooldown;
    protected bool _isAttacking = false;

    protected enum ScreenSide
    {
        Left,
        Right,
    }

    protected abstract ScreenSide CheckTargetPosition();
    protected abstract void OrientateTowardsTarget();
    protected void StartAttack()
    {
        if (_isAttacking) return;
        StartCoroutine(Attack());
    }
    protected virtual IEnumerator Attack()
    {
        _isAttacking = true;
        yield return new WaitForSeconds(_windup);
        _weaponHitbox.SetActive(true);
        yield return new WaitForSeconds(_attackTime);
        _weaponHitbox.SetActive(false);
        yield return new WaitForSeconds(_cooldown);
        _isAttacking = false;
    }
}
