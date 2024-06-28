using UnityEngine;


public class Bullet : MonoBehaviour
{
    public float damage = 10f; // obra?enia zadawane przez pocisk

    void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject); // zniszcz pocisk po trafieniu
        }
    }
}
