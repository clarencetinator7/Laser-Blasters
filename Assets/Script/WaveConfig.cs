using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Configuration")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefabs;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeed = 2f;

    public GameObject GetEnemyPrefab() { return enemyPrefab; }
    public List<Transform> GetWaypoints() 
    {
        var waveWaypoints = new List<Transform>();

        foreach (Transform child in pathPrefabs.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints;
    }
    public float GetTimeBetweenSpawns() { return timeBetweenSpawns; }
    public float GetSpawnRandomFactor() { return spawnRandomFactor; }
    public int GetNumberOfEnemies() { return numberOfEnemies; }
    public float GetMoveSpeed() { return moveSpeed; }

}
