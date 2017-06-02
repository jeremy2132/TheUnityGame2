using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

    private Rigidbody2D myRigidbody;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float dashUp;
    [SerializeField]
    private float dashDown;
    [SerializeField]
    private float Horizontaldash;

    int amountJumped;
    int amountJumped2;
    int amountDashed;
    int amountDashed2;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool grounded;
    private bool doubleJumped;
    private bool invcibleDash;

 
    public SpriteRenderer dashCircle;

    // Use this for initialization
    void Start()
    {
        
        dashCircle = GetComponent<SpriteRenderer>();
        stats.Init();
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.drag = 15;
    }

    private void FixedUpdate()
    {
        //grounded
        Grounded2();

        // Moving In FixedUpdate
        if (Input.GetKey(KeyCode.A))
        {
            HorizontalSpeedLeft();
        }

        if (Input.GetKey(KeyCode.D))
        {
            HorizontalSpeedRight();
        }

        //Jumping
        if (Input.GetKey(KeyCode.Space) && amountJumped > 0)
        {
            amountJumped--;
            amountJumped2 = 1;
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.Space) && amountJumped2 > 0)
        {
            amountJumped2--;
            Jump();
        }

        //Dashing while grounded
        if (Input.GetKey(KeyCode.D) && Input.GetMouseButtonDown(1) && grounded)
        {
            DashHorizontalRight();
        }

        if (Input.GetKey(KeyCode.A) && Input.GetMouseButtonDown(1) && grounded)
        {
            DashHorizontalLeft();
        }

        //Dashing in FixedUpdate
        if (Input.GetKey(KeyCode.W) && Input.GetMouseButton(1) && amountDashed > 0)
        {
            DashVerticalUp();
            amountDashed--;
            amountDashed2 = 1;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetMouseButton(1) && amountDashed > 0)
        {
            DashVerticalDown();
            amountDashed--;
            amountDashed2 = 1;
        }

        if (Input.GetKey(KeyCode.D) && Input.GetMouseButton(1) && !grounded && amountDashed > 0)
        {
            DashHorizontalRight();
            amountDashed--;
            amountDashed2 = 1;
        }

        if (Input.GetKey(KeyCode.A) && Input.GetMouseButton(1) && !grounded && amountDashed > 0)
        {
            DashHorizontalLeft();
            amountDashed--;
            amountDashed2 = 1;
        }
        //Double Dash
        if (Input.GetKey(KeyCode.W) && Input.GetMouseButtonDown(1) && amountDashed2 > 0)
        {
            DashVerticalUp();
            amountDashed2--;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetMouseButtonDown(1) && amountDashed2 > 0)
        {
            DashVerticalDown();
            amountDashed2--;
        }

        if (Input.GetKey(KeyCode.D) && Input.GetMouseButtonDown(1) && amountDashed2 > 0)
        {
            DashHorizontalRight();
            amountDashed2--;
        }

        if (Input.GetKey(KeyCode.A) && Input.GetMouseButtonDown(1) && amountDashed2 > 0)
        {
            DashHorizontalLeft();
            amountDashed2--;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (grounded)
        {
            Grounded();
        }


        // Invincible Dash
        StartCoroutine(InvincibleDashTime());

        ///////////////Player Stats////////////////
        if (transform.position.y <= fallBoundry)
        {
            DamagePlayer(99999);
        }

    }
    //Moving and Jumping Methods
    public void HorizontalSpeedLeft()
    {
        myRigidbody.velocity = new Vector2(-moveSpeed, myRigidbody.velocity.y);
    }

    public void HorizontalSpeedRight()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
    }

    public void Jump()
    {
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpHeight);
    }

    //Dashing Methods
    public void DashVerticalUp()
    {
        invcibleDash = true;
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, dashUp);
    }

    public void DashVerticalDown()
    {
        invcibleDash = true;
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, -dashDown);
    }

    public void DashHorizontalRight()
    {
        invcibleDash = true;
        myRigidbody.velocity = new Vector2(Horizontaldash, myRigidbody.velocity.y);
    }

    public void DashHorizontalLeft()
    {
        invcibleDash = true;
        myRigidbody.velocity = new Vector2(-Horizontaldash, myRigidbody.velocity.y);
    }

    //Grounded
    private void Grounded()
    {
        amountJumped = 2;
        amountJumped2 = 1;
        amountDashed = 2;
        amountDashed2 = 1;
    }
    private void Grounded2()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    /////////////////////////////////////////Player Stats//////////////////////////////////////////////////////////////////////////////

    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth = 3;
        private int _currentHealth;
        public int currentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            currentHealth = maxHealth;
        }
    }

    public PlayerStats stats = new PlayerStats();

    public int fallBoundry;
    public int damage;

    public void DamagePlayer(int damage)
    {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            GameMaster.KillPlayer(this);
        }
    }

    //Collision with Enemy
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "EnWeapon" && invcibleDash == false)
        {
            DamagePlayer(damage);
        }
    }

    IEnumerator InvincibleDashTime()
    {
        if (invcibleDash == true)
        {
            dashCircle.sprite = Resources.Load<Sprite>("Spr_PlayerCircle");
            yield return new WaitForSeconds(.1f);
            invcibleDash = false;
            dashCircle.sprite = Resources.Load<Sprite>("Spr_Player");
            yield break;
        }

    }

}

