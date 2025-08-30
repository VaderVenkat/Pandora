using UnityEngine;
using System.Collections;

public class Health_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject healthPrefab;       // assign your Health_Behaviour prefab
    [SerializeField] private Vector3[] spawnPoints;       // set 3 spawn positions in Inspector
    [SerializeField] private float respawnTime = 15f;

    private GameObject[] currentHealthItems;

    void Start()
    {
        currentHealthItems = new GameObject[spawnPoints.Length];

        // Spawn all health items at start
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnHealth(i);
        }
    }

    void SpawnHealth(int index)
    {
        // If already exists, do nothing
        if (currentHealthItems[index] != null) return;

        GameObject newHealth = Instantiate(healthPrefab, spawnPoints[index], Quaternion.identity);

        // Tell health object which spawner created it
        Health_Behaviour hb = newHealth.GetComponent<Health_Behaviour>();
        if (hb != null)
        {
            hb.Init(this, index);
        }

        currentHealthItems[index] = newHealth;
    }

    public void RespawnHealth(int index)
    {
        StartCoroutine(RespawnAfterDelay(index));
    }

    private IEnumerator RespawnAfterDelay(int index)
    {
        yield return new WaitForSeconds(respawnTime);
        SpawnHealth(index);
    }
}
