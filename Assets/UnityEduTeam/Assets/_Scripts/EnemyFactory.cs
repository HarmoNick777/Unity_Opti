using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : Factory
{
    [SerializeField] GameObject[] enemySpawnPoints;
    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] Pool enemyPool;
    public int enemiesToSpawn;
    
    void Start()
    {
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            
            //Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], enemySpawnPoints[i].transform.position, enemySpawnPoints[i].transform.rotation);
        }
        enemiesToSpawn = 0;
    }

    void Update()
    {
        // vérification si un enemy est mort et le cas échéant en faire spawn un nouveau à une position aléatoire
        // pour cela on compare le nombre théorique d'enemy avec le nombre actuel
        while (enemiesToSpawn > 0)
        {
            int randomSpawnPoint = Random.Range(0, enemySpawnPoints.Length);
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)],
                enemySpawnPoints[randomSpawnPoint].transform.position,
                enemySpawnPoints[randomSpawnPoint].transform.rotation);
        }
    }

    public override Transform Generate(Vector3 position, Quaternion rotation)
    {
        int randomSpawnPoint = Random.Range(0, enemySpawnPoints.Length);
        GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)],
                enemySpawnPoints[randomSpawnPoint].transform.position,
                enemySpawnPoints[randomSpawnPoint].transform.rotation);
        return enemy.transform;
    }
}
