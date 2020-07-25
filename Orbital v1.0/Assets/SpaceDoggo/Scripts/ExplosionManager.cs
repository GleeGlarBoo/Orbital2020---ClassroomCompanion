using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    #region Singleton
    public static ExplosionManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }
    #endregion

    public GameObject ExplosionWithRewardPrefab;            // for asteroids
    public GameObject NormalExplosionPrefab;                // for bullet and player

    public void SpawnExplosionWithRewardAt(Vector2 position, float scale)
    {
        GameObject explosion = Instantiate(ExplosionWithRewardPrefab, position, Quaternion.identity);

        // Rotate around z-axis by a random amount
        explosion.transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));

        // Scale the explosion by the input parameter, with a little bit of randomness
        explosion.transform.localScale = Vector2.one * scale * Random.Range(0.9f, 1.1f);

        // Destroy the explosion after the animation has finished playing
        Destroy(explosion, 3f);
    }

    public void SpawnExplosionAt(Vector2 position, float scale)
    {
        GameObject explosion = Instantiate(NormalExplosionPrefab, position, Quaternion.identity);

        // Rotate around z-axis by a random amount
        explosion.transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));

        // Scale the explosion by the input parameter, with a little bit of randomness
        explosion.transform.localScale = Vector2.one * scale * Random.Range(0.9f, 1.1f);

        // Destroy the explosion after the animation has finished playing
        Destroy(explosion, 3f);
    }

}
