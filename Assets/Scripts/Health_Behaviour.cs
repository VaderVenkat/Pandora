using UnityEngine;

public class Health_Behaviour : MonoBehaviour
{
    private Transform _transform;
    private float rotatespeed = 75f;

    [HideInInspector] public Health_Spawner spawner;
    [HideInInspector] public int spawnIndex;

    public void Init(Health_Spawner spawner, int index)
    {
        this.spawner = spawner;
        this.spawnIndex = index;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _transform.Rotate( rotatespeed * Time.deltaTime,0,0);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                player.AddLife(4);
            }

            // Tell spawner to respawn this item later
            if (spawner != null)
            {
                spawner.RespawnHealth(spawnIndex);
            }

            Destroy(gameObject);
        }
    }


}
