using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_Shooting : MonoBehaviour
{
    private Transform player;
    [SerializeField] private GameObject Bullet_prefab;
    private Transform Head_Transform;

    [SerializeField] private float shootDistance = 10f;
    [SerializeField] private float shootInterval = 0.5f;
    [SerializeField] private float Bullet_speed = 15f;

    private float shootTimer;

    private void Start()
    {
        player = GameObject.Find("Player").transform;

        Head_Transform = transform.Find("Head");
    }

    private void Update()
    {
        if (player == null || Head_Transform == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= shootDistance)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer >= shootInterval)
            {
                shootTimer = 0;
                ShootAtPlayer();
            }
        }
    }

    void ShootAtPlayer()
    {
        Vector3 shootdir = (player.position - Head_Transform.position).normalized;

        GameObject bullets = Instantiate(Bullet_prefab, Head_Transform.position, Quaternion.identity);

        Rigidbody _rb = bullets.GetComponent<Rigidbody>();

        if(_rb != null)
        {
            _rb.linearVelocity = shootdir * Bullet_speed;
        }

        Destroy(bullets, 2f);
    }
}
