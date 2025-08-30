using UnityEngine;

public class Enemy_Chasing : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 8f;
    public float stopDistance = 1.5f;
    public int lives = 1;

    private Enemy_Spawn spawner;
    private int spawnIndex;

    public void Init(Transform target, Enemy_Spawn spawnerRef, int index)
    {
        player = target;
        spawner = spawnerRef;
        spawnIndex = index;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            Vector3 moveDir = direction.normalized;
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(moveDir) * Quaternion.Euler(0f, -90f, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet_Player"))
        {
            lives -= 1;

            if (lives <= 0)
            {
                spawner.RespawnEnemy(spawnIndex); // Tell spawner which slot to respawn
                Destroy(gameObject);
            }
        }
    }
}
