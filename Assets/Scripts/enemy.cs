using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public float health = 50f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        //GetComponent<EnemyDrop>().DropResources();
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Enemy otherEnemy = collision.gameObject.GetComponent<Enemy>();
        if (otherEnemy != null)
        {
            // Implementacja logiki interakcji ?ywio?ów
            HandleElementalInteraction(otherEnemy);
        }
    }

    void HandleElementalInteraction(Enemy otherEnemy)
    {
        // Cykl zwyci??ania
        if ((enemyType == EnemyType.Metal && otherEnemy.enemyType == EnemyType.Wood) ||
            (enemyType == EnemyType.Wood && otherEnemy.enemyType == EnemyType.Earth) ||
            (enemyType == EnemyType.Earth && otherEnemy.enemyType == EnemyType.Water) ||
            (enemyType == EnemyType.Water && otherEnemy.enemyType == EnemyType.Fire) ||
            (enemyType == EnemyType.Fire && otherEnemy.enemyType == EnemyType.Metal))
        {
            otherEnemy.TakeDamage(20f); // zadanie obra?e? przeciwnikowi, który przegra?
        }
    }
}
