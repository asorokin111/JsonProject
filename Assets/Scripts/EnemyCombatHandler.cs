using UnityEngine;

public class EnemyCombatHandler : CombatHandler
{
    private Transform _player;
    private ScreenSide _previousPlayerSide = ScreenSide.Left;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (_isAttacking) return;
        ScreenSide relativePlayerSide = CheckTargetPosition();
        if (relativePlayerSide != _previousPlayerSide)
        {
            OrientateTowardsTarget();
        }
        _previousPlayerSide = relativePlayerSide;
        StartAttack();

    }

    // Checks whether the player is on the left or right side relative to this enemy
    protected override ScreenSide CheckTargetPosition()
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

    protected override void OrientateTowardsTarget()
    {
        Transform enemyTransform = transform.parent;
        enemyTransform.localScale = new Vector3(
                    -enemyTransform.localScale.x,
                    enemyTransform.localScale.y,
                    enemyTransform.localScale.z
                );
    }
}
