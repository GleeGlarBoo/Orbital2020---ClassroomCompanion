using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] Objects;

    // Random range for more randomness to the game. Initially was pre-set to 1.2
    private float m_SpawnRate = 1.2f;

    private float m_TimeSinceLastSpawn;

    public bool startGame = false;

    void Update()
    {
        if (startGame)
        {
            if ((Time.time - m_TimeSinceLastSpawn > m_SpawnRate) && (!Player_FL.instance.Health_Script.IsDead()))           // if player not dead too. If player dead, dont spawn anymore
            {
                m_TimeSinceLastSpawn = Time.time;
                m_SpawnRate = Random.Range(2f, 4f);

                SpawnAtRandomPosition();
            }

        }

    }

    void SpawnAtRandomPosition()
    {
        int randomEnemyIndex = Random.Range(0, Objects.Length);
        Vector2 spawnPosition = transform.position;

        // only allow coin to be spanwed in the air, or on the ground, the other 2 will be on the ground
        if (randomEnemyIndex == 0 || randomEnemyIndex == 3)             // since we got 2 coin prefabs in the array now
        {
            float probability = Random.value;
            if (probability <= 0.70f)           // 70 % chance of coin in the air
            {
                spawnPosition.y = 1.45f;

            }
            else
            {
                spawnPosition.y = -1.30f;
            }
        }
        // for the other 2 objects, they will just spawn at the position of the spawner, will touch the ground before they are seen by the user due to gravity
        Instantiate(Objects[randomEnemyIndex], spawnPosition, Quaternion.identity);
    }


    public void StartGameAfterInstruction()
    {
        startGame = true;
    }
}
