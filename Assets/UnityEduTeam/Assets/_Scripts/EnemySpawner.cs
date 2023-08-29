using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> EnemyPrefabs;
    private GameObject[] enemySpawnPoints;
    
    void Start()
    {
        enemySpawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");

        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)], enemySpawnPoints[i].transform.position, enemySpawnPoints[i].transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // vérification si un enemy est mort et le cas échéant en faire spawn un nouveau à une position aléatoire
        // pour cela on compare le nombre théorique d'enemy avec le nombre actuel
        while (enemySpawnPoints.Length >
               GameObject.FindGameObjectsWithTag("Enemy").Length)
        {
            int RandomNumber = Random.Range(0, EnemyPrefabs.Count);
            Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)],
                enemySpawnPoints[RandomNumber].transform.position,
                enemySpawnPoints[RandomNumber].transform.rotation);
        }
    }
}
