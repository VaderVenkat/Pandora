using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 7f;

    private Rigidbody rb;

    private float VInput;
    private float HInput;

    // Jumping actions

    [SerializeField] private float JumpVelocity = 8f;
    private bool _isJumping;

    // Layer Mask 

    [SerializeField] private float DistanceToGround = 0.1f;
    [SerializeField] private LayerMask GroundLayer;
    private SphereCollider _col;

    // color swaping 

    private Renderer _playerrender;

    // reference to the player'monoloth

    [SerializeField] private GameObject playerMonolith;

    private Renderer playerMonolithRenderer;

   

    
    // Shooting 

    private bool _isShooting;
    private float Bulletspeed = 100f;
    [SerializeField] private GameObject Bulletprefab;

    // Lives

    [SerializeField] private int player_life = 10;

    private UI_Behaviour game_manager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        _col = GetComponent<SphereCollider>();

        _playerrender = GetComponent<Renderer>();

        game_manager = GameObject.Find("Game_Manager").GetComponent<UI_Behaviour>();

        game_manager.UpdateHealth(player_life);

      
         playerMonolithRenderer = playerMonolith.GetComponentInChildren<Renderer>();



    }

    // Update is called once per frame
    private void Update()
    {
        _isJumping |= Input.GetButtonDown("Jump");

        _isShooting |= Input.GetButtonDown("Fire1");
    }

    private void FixedUpdate()
    {
        VInput = Input.GetAxis("Vertical");

        HInput = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(HInput, 0.0f, VInput);

        if (movement.magnitude >= 0.1f)
        {
            // Get camera forward & right (ignore vertical tilt)
            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = Camera.main.transform.right;
            camRight.y = 0f;
            camRight.Normalize();

            // Move direction relative to camera
            Vector3 moveDir = camForward * VInput + camRight * HInput;

            // Apply force to roll ball
            rb.AddForce(moveDir * MoveSpeed);
        }

        // Jumping mechanish

        if (IsGrounded() && _isJumping)
        {
            rb.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);
            Debug.Log("the J is Pressed");
        }
        _isJumping = false;


        // shooting mechanism

        if (_isShooting)
        {
            GameObject bullets = Instantiate(Bulletprefab, transform.position + Vector3.up + Vector3.forward * 1f, transform.rotation);

            Rigidbody bulletrb = bullets.GetComponent<Rigidbody>();

            Vector3 shootDir = Camera.main.transform.forward;

            shootDir.y = 0f;

            bulletrb.linearVelocity = shootDir.normalized * Bulletspeed;

            Destroy(bullets, 5f);
        }

        _isShooting = false;
    }

    // cheching the ground layer
    private bool IsGrounded()
    {
        Vector3 sphereBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);

        bool grounded = Physics.CheckSphere(sphereBottom, DistanceToGround, GroundLayer, QueryTriggerInteraction.Ignore);

        return grounded;
    }

  

    private void OnCollisionEnter(Collision collision)
    {
      

        Color_Swaping(collision);   // color swaping 

        Show_winButton(collision); // show win buttonl
    }

    

    private void OnTriggerEnter(Collider other)
    {
        player_damage(other); // attacked by enemy 

     

    }

    // show win button
    private void Show_winButton(Collision collision)
    {
        if (playerMonolithRenderer != null && IsViolet(playerMonolithRenderer.material.color))
        {
            game_manager.ShowWin();
        }
    }

    private bool IsViolet(Color color)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);

        // Violet/purple hue is roughly between 0.72–0.82 (260°–295° on color wheel)
        return (h >= 0.72f && h <= 0.82f);
    }

   

    private void player_damage(Collider other)
    {
        if (other.CompareTag("Bullet_Enemy"))
        {
            TakeDamage(1);
        }

    }


    private void Color_Swaping(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monolith") || collision.gameObject.CompareTag("Goal"))
        {
            Renderer MonolithRender = collision.gameObject.GetComponent<Renderer>();

            if (MonolithRender != null)
            {
                Color tempcolor = _playerrender.material.color;
                _playerrender.material.color = MonolithRender.material.color;
                MonolithRender.material.color = tempcolor;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        player_life -= amount;

        game_manager.UpdateHealth(player_life);

        if (player_life <= 0)
        {
            game_manager.ShowGameOver();
            Destroy(gameObject);
        }
    }

    // life support 
    public void AddLife(int amount = 1)
    {

        player_life += amount;

        // Optional: cap max lives

        if (player_life > 10) // you can change max limit
            player_life = 10;

        game_manager.UpdateHealth(player_life);

        Debug.Log("Life Added! Current Lives = " + player_life);
    }


   
}
