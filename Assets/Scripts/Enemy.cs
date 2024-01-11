using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public void Damage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
