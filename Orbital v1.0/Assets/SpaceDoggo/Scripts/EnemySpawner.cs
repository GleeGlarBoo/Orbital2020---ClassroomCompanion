using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] m_Enemies;

    // Random range for more randomness to the game. Initially was pre-set to 1.2
    private float m_SpawnRate = 1.2f;

    private float m_TimeSinceLastSpawn;

    void Update()
    {
        // start game 2 secs late
        Invoke("spawnEnemies", 3.5f);
    }

    void SpawnAtRandomPosition()
    {
        int randomEnemyIndex = Random.Range(0, m_Enemies.Length);
        Vector2 spawnPosition = transform.position;
        spawnPosition.x += Random.Range(-2.0f, 2.0f);
        Instantiate(m_Enemies[randomEnemyIndex], spawnPosition, Quaternion.identity);
    }


    void spawnEnemies()
    {
        if (Time.time - m_TimeSinceLastSpawn > m_SpawnRate)
        {
            m_TimeSinceLastSpawn = Time.time;
            m_SpawnRate = Random.Range(0.6f, 1.5f);

            SpawnAtRandomPosition();
        }    
    }
}
