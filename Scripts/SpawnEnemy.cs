using System.Collections;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private Transform[] EnemySpawnPoints;
    [SerializeField] private GameObject Enemy = default;

    public float EnemySpawnTime = 10;

    private void Start()
    {
        StartCoroutine(EnemySpawner());
    }



    
    IEnumerator EnemySpawner()
    {
        while (true) {

            yield return new WaitForSeconds(EnemySpawnTime);
            Instantiate(Enemy, EnemySpawnPoints[Random.Range(1,EnemySpawnPoints.Length-1)]);
        }
       
            
    }
}
