using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    [SerializeField]
    private int _damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemyScript = other.gameObject.GetComponent<Enemy>();
            enemyScript.Damage(_damage);
        }
    }
}
