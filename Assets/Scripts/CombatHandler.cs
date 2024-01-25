using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatHandler : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField]
    protected float _windup;
    [SerializeField]
    protected float _attackTime;
    [SerializeField]
    protected float _cooldown;

    protected enum ScreenSide
    {
        Left,
        Right,
    }

    protected abstract ScreenSide CheckTargetPosition();
    protected abstract void OrientateTowardsTarget();
}
