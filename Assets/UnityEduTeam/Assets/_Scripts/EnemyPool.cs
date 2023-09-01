using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : Pool
{
    [SerializeField] Factory enemyFactory;

    private IObjectPool<Transform> enemies;
    
    void Start()
    {
        enemies = new ObjectPool<Transform>(CreateNewEnemy, OnGettingEnemy, OnReleasingEnemy, OnDestroyingEnemy);
    }

    private Transform CreateNewEnemy()
    {
        return enemyFactory.Generate(Vector3.zero, Quaternion.identity);
    }

    private void OnGettingEnemy(Transform enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnReleasingEnemy(Transform enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyingEnemy(Transform enemy)
    {
        Destroy(enemy);
    }

    public override Transform Get(Vector3 position, Quaternion rotation)
    {
        Transform t = enemies.Get();
        t.position = position;
        t.GetComponent<Rigidbody>().velocity = Vector3.zero;
        t.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        return t;
    }

    public override void Release(Transform element)
    {
        enemies.Release(element);
    }
}
