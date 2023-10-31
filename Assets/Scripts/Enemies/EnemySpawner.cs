using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float timeBetweenWaves;
    [SerializeField] float spaceBetweenEnemies;
   
    private Transform spawnLocation;
    private float waveTimer = 2;
    private int currentWaveSize = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (waveTimer <= 0)
        {
            StartCoroutine(SpawnWave());
            waveTimer = timeBetweenWaves;
        }
        waveTimer -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        currentWaveSize++;

        for (int i = 0; i < currentWaveSize; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spaceBetweenEnemies);
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnLocation.position, spawnLocation.rotation);
    }
}
