using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatHandler : MonoBehaviour
{
    [Tooltip("Weapon hitbox object")]
    [SerializeField]
    private GameObject _hitbox;
    // Somewhat similar to the player's combat handler
    [Header("Combat Settings")]
    [SerializeField]
    private float _windup;
    [SerializeField]
    private float _attackTime;
    [SerializeField]
    private float _cooldown;

    private Transform _player;
    private ScreenSide _previousPlayerSide = ScreenSide.Left;
    private bool _isAttacking;

    enum ScreenSide
    {
        Left,
        Right,
    }

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (_isAttacking) return;
        ScreenSide relativePlayerSide = CheckPlayerSide();
        if (relativePlayerSide != _previousPlayerSide)
        {
            Flip();
        }
        _previousPlayerSide = relativePlayerSide;
        StartAttack();

    }

    // Checks whether the player is on the left or right side relative to this enemy
    ScreenSide CheckPlayerSide()
    {
        if (_player.position.x >= transform.position.x)
        {
            return ScreenSide.Right;
        }
        else
        {
            return ScreenSide.Left;
        }
    }

    private void Flip()
    {
        Transform enemyTransform = transform.parent;
        enemyTransform.localScale = new Vector3(
                    -enemyTransform.localScale.x,
                    enemyTransform.localScale.y,
                    enemyTransform.localScale.z
                );
    }

    private void StartAttack()
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
