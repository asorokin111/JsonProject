using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    [SerializeField]
    private int _damage;
    [SerializeField]
    private ParticleSystem _hitParticles;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemyScript = other.gameObject.GetComponent<Enemy>();
            enemyScript.Damage(_damage);
            _hitParticles.transform.position = other.transform.position;
            if (_hitParticles.isPlaying) _hitParticles.Stop();
            _hitParticles.Play();
        }
    }
}
