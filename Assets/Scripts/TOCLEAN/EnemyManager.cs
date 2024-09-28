// using UnityEngine;

// public class EnemyManager : MonoBehaviour
// {
//     public GameObject[] waterEnemies; // przeciwnicy wody
//     public GameObject[] fireEnemies;  // przeciwnicy ognia
//     public GameObject[] woodEnemies;  // przeciwnicy drewna
//     public GameObject[] metalEnemies; // przeciwnicy metalu
//     public GameObject[] earthEnemies; // przeciwnicy ziemi

//     public Transform[] spawnPoints;

//     void Start()
//     {
//         SpawnEnemies();
//     }

//     void SpawnEnemies()
//     {
//         SpawnElementalEnemies(waterEnemies);
//         SpawnElementalEnemies(fireEnemies);
//         SpawnElementalEnemies(woodEnemies);
//         SpawnElementalEnemies(metalEnemies);
//         SpawnElementalEnemies(earthEnemies);
//     }

//     void SpawnElementalEnemies(GameObject[] enemies)
//     {
//         foreach (Transform spawnPoint in spawnPoints)
//         {
//             int randomIndex = Random.Range(0, enemies.Length);
//             Instantiate(enemies[randomIndex], spawnPoint.position, spawnPoint.rotation);
//         }
//     }
// }
