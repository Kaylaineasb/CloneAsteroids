using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawn : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public float spawnRate = 8f;
    public float spawnDistance = 10f;

    void Start()
    {
        InvokeRepeating("SpawnAsteroid", 0f, spawnRate);
    }

   void SpawnAsteroid()
{
    Vector2 spawnPosition = Random.insideUnitCircle.normalized * spawnDistance;
    GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

    float randomSize = Random.Range(3.0f, 5.0f);
    Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();
    if (asteroidScript != null)
    {
        asteroidScript.SetSize(randomSize);  // Define o tamanho aleatório
        asteroidScript.SetDirection(Random.insideUnitCircle.normalized);  // Direção aleatória
        asteroidScript.SetMovement();  // Inicializa o movimento
    }
    else
    {
        Debug.LogError("Prefab do asteroide não possui o script Asteroid!");
    }
}
}


