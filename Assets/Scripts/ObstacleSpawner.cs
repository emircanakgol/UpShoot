using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    public static ObstacleSpawner instance;
    
    [Header("Prefabs")] 
    [SerializeField] private GameObject obstaclePrefab;
    public List<GameObject> fruitPrefabs;

    [Header("Settings")] 
    [SerializeField] private int obstacleCount;
    public float hitTweenDuration;

    private void Awake() {
        if (instance == null)
            instance = this;
    }

    private void Start() {
        SpawnObstacles();
    }

    private void SpawnObstacles() {
        float spawnStep = UpgradeSpawner.instance.TrackLength / obstacleCount;
        for (int i = 1; i < obstacleCount; i++) {
            
            if (i % UpgradeSpawner.instance.UpgradeCount == 0) continue;
            
            int spawnCount = 0;
            for (int j = -1; j < 2; j++) {
                if (i != obstacleCount - 1) {
                    if (spawnCount == 2) break;
                    if (Random.Range(0, 2) == 1) continue;
                }
                Vector3 spawnPos = new Vector3(1.1f * j, 0, spawnStep * i);
                GameObject obstacleGO = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity, transform);
                GameObject obstacleFruitGO = Instantiate(SelectFruit(i), obstacleGO.transform);
                obstacleFruitGO.transform.Rotate(Vector3.up, Random.Range(0, 360));
                Obstacle obstacle = obstacleGO.GetComponent<Obstacle>();
                obstacle.Difficulty = i;
                spawnCount++;
            }

            if (spawnCount == 0) {
                Vector3 spawnPos = new Vector3(1.1f * Random.Range(-1, 2), 0, spawnStep * i);
                GameObject obstacleGO = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity, transform);
                GameObject obstacleFruitGO = Instantiate(SelectFruit(i), obstacleGO.transform);
                obstacleFruitGO.transform.Rotate(Vector3.up, Random.Range(0, 360));
                Obstacle obstacle = obstacleGO.GetComponent<Obstacle>();
                obstacle.Difficulty = i;
                spawnCount++;
            }
        }
    }

    private GameObject SelectFruit(int difficulty) {
        int maxDifficulty = obstacleCount - 1;
        float difficultyRate = (float)difficulty / maxDifficulty;
        if (difficultyRate <= 0.2f)
            return fruitPrefabs[0];
        else if(difficultyRate <= 0.4f)
            return fruitPrefabs[1];
        else if(difficultyRate <= 0.6f)
            return fruitPrefabs[2];
        else if(difficultyRate <= 0.8f)
            return fruitPrefabs[3];
        else
            return fruitPrefabs[4];
    }
}