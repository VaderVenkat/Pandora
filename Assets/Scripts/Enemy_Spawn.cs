using UnityEngine;
using System.Collections;

public class Enemy_Spawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private Vector3[] spawnPositions;      // Each slot has a spawn point
    [SerializeField] private float respawnDelay = 3f;

    private GameObject[] currentEnemies;  // Track spawned enemies per slot

    void Start()
    {
        currentEnemies = new GameObject[spawnPositions.Length];

        // Spawn one enemy at each spawn position
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            SpawnEnemyAt(i);
        }
    }

    void SpawnEnemyAt(int index)
    {
        if (currentEnemies[index] == null) // Only spawn if slot empty
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPositions[index], Quaternion.identity);

            Enemy_Chasing chaseScript = newEnemy.GetComponent<Enemy_Chasing>();
            if (chaseScript != null)
            {
                chaseScript.Init(player, this, index); // Pass index
            }

            currentEnemies[index] = newEnemy; // Save reference
        }
    }

    // Called by Enemy when it dies
    public void RespawnEnemy(int index)
    {
        StartCoroutine(RespawnAfterDelay(index));
    }

    IEnumerator RespawnAfterDelay(int index)
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnemyAt(index);
    }
}
