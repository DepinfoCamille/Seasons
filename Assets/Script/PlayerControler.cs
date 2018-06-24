using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float maxSpeed = 10f;    // max horizontal speed
    protected bool airControl = true;   // whehter control is allowed during jump phase
    public LayerMask groundLayers;  // layer used to test for the ground, should be defined through the inspector
    public int type; // 1=Automn, 2=Spring, 3= Summer, 4=Winter
    // Current state of the player
    private bool grounded;  // true if player is considered grounded
    public bool facingRight = true;    // facing direction of the player
    private bool walledL;
    private bool walledR; // true if player is considered as hiting the ground

    private bool jump;  // save jump button status for fixed update
    private float TheTime;
    // store Component of the Player GameObject that need to be used in the script
    private Rigidbody2D m_Rigidbody;
    private CapsuleCollider2D m_Capsule;
    private Animator m_Animator;
    
    // HashId to manage the animation (faster than sting based approach)
    protected readonly int m_HashGroundedPara = Animator.StringToHash("grounded");
    protected readonly int m_HashSpeedPara = Animator.StringToHash("speed");
    protected readonly int m_HashVerticalSpeedPara = Animator.StringToHash("vSpeed");

    // Contacts (we do not want to re-affects memory at every frame)
    public float groundedRaycastDistance = 0.1f;
    ContactFilter2D m_ContactFilter;
    RaycastHit2D[] m_HitBuffer = new RaycastHit2D[5];

    private void Awake()
    {
        // get the component that will be used at each Update
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Capsule = GetComponent<CapsuleCollider2D>();
        TheTime = 0f;
        // define behavior for raycasting
        m_ContactFilter.layerMask = groundLayers;
        m_ContactFilter.useLayerMask = true;
        m_ContactFilter.useTriggers = false;
        Physics2D.queriesStartInColliders = false; // do not take into account collider within which we are starting the raycast
    }

    // Update is called once per frame
    public void SetPlayerNumber(int i)
    {
        type = i;
    }
    void Update()
    {
        if (!jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            jump = Input.GetButtonDown("Jump");
        }
        switch (type) {
            case (1):
                CheckAndThrowBomb_A();
                break;
            case (2):
                CheckAndThrowBomb_P();
                break;
            default:
                break;
        }
        TheTime=Time.time;

    }

    // Fixed Update emulate constant time steps for physic engine
    private void FixedUpdate()
    {
        // read continous inputs to obtain smooth motions
        float h = Input.GetAxis("Horizontal");

        // check whether we are grounded or not, and update player status accordingly
        CheckIfGrounded();

        // pass movement parameters to function that manage actual movement
        Move(h, jump);
        jump = false;

        // update the state of the animation state machine
        m_Animator.SetBool(m_HashGroundedPara, grounded);
        m_Animator.SetFloat(m_HashSpeedPara, Mathf.Abs(h));
        // we handle both fall and jump animation based on the same animation 
        // that is parametrized by the vertical speed (see blend tree in Player Animator)
        m_Animator.SetFloat(m_HashVerticalSpeedPara, m_Rigidbody.velocity.y);
    }

    public void Move(float move, bool jump)
    {
        if (grounded || airControl)
        {
            m_Rigidbody.velocity = new Vector2(move * maxSpeed, m_Rigidbody.velocity.y);

            // update facing of the player using the sprite to simplify the Animator
            if ((move > 0 && !facingRight) || (move < 0 && facingRight))
            {
                Flip();
            }


            if (jump)
            {
                if (grounded)
                {
                    grounded = false;
                    m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x / 1.5f, m_Rigidbody.velocity.y / 2 + maxSpeed / 1.5f);
                }
                else
                {
                    if (walledL && Input.GetKey("right"))
                    {
                        m_Rigidbody.velocity = new Vector2(0, m_Rigidbody.velocity.y / 2 + maxSpeed);
                        Flip();
                    }
                    if (walledR && Input.GetKey("left"))
                    {
                        m_Rigidbody.velocity = new Vector2(0, m_Rigidbody.velocity.y / 2 + maxSpeed);
                        Flip();
                    }
                }
            }
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
        // can also be done using the flip attribut of the SpriteRenderer
    }

    public void CheckIfGrounded()
    {
        // Compute raycast starting point and direction, such that the ray
        // is directed toward the ground and have a length of groundedRaycastDistance outside
        // the capsule collider of the character

        Vector2 raycastDirection = Vector2.down;
        Vector2 raycastStart = m_Rigidbody.position + m_Capsule.offset;
        raycastStart = raycastStart + Vector2.down * (m_Capsule.size.y * 0.5f - m_Capsule.size.x * 0.5f);
        float raycastDistance = m_Capsule.size.x * 0.5f + groundedRaycastDistance * 2.0f;

        int count = Physics2D.Raycast(raycastStart, raycastDirection, m_ContactFilter, m_HitBuffer, raycastDistance);

        Vector2 raycastDirectionL = Vector2.left;
        Vector2 raycastStartL = m_Rigidbody.position + m_Capsule.offset;
        float raycastDistanceL = m_Capsule.size.y * 0.5f + groundedRaycastDistance * 2.0f;
        Vector2 raycastDirectionR = Vector2.right;
        Vector2 raycastStartR = m_Rigidbody.position + m_Capsule.offset;
        float raycastDistanceR = m_Capsule.size.y * 0.5f + groundedRaycastDistance * 2.0f;

        int countwallL = Physics2D.Raycast(raycastStartL, raycastDirectionL, m_ContactFilter, m_HitBuffer, raycastDistanceL);
        int countwallR = Physics2D.Raycast(raycastStartR, raycastDirectionR, m_ContactFilter, m_HitBuffer, raycastDistanceR);
        // We can check the ray that will be sent in the scene for debugging
        //Debug.DrawRay (raycastStart, raycastDirection); // * raycastDistance

        if (count > 0)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        if (countwallL > 0)
        {
            walledL = true;
        }
        else
        {
            walledL = false;
        }
        if (countwallR > 0)
        {
            walledR = true;
        }
        else
        {
            walledR = false;
        }

        //m_HitBuffer[0] contains informations on the closest collider 

        //free memory
        for (int i = 0; i < m_HitBuffer.Length; i++)
        {
            m_HitBuffer[i] = new RaycastHit2D();
        }
    }


    // Using a coroutine to fire bombs at fixed time steps when the fire button is pressed

    protected float time_shot;
    protected float shot_delay=0.5f;
    protected bool cloud_perm = false;
    protected bool fire = false;
    protected float last_shot=0;
    protected bool pressed = false;
    protected bool fired = false;
    protected float latence = 0f;

    //###### SPRING #########
    private void CheckAndThrowBomb_P()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(Time.time-latence> 1f)
            {
                StartCoroutine(SpawnBuisson());
                latence = Time.time;
            }
        }
    }
    public GameObject buisson;
    IEnumerator SpawnBuisson()
    {
        Vector2 posStart = m_Rigidbody.position + m_Capsule.offset + Vector2.left * (facingRight ? -1f : 1f) * m_Capsule.size.x + new Vector2(0, 1);
        GameObject bui = Instantiate(buisson, posStart, new Quaternion(0, 0, 0, 0)); // Create a new game object from the prefab
        bui.SetActive(true); // ensure the new object is Active to be visible within the scene
        yield return new WaitForSeconds(5f);
        Destroy(bui);
    }
    //###### AUTUMN #########
    protected void CheckAndThrowBomb_A()
    {
        if (Input.GetButtonDown("Fire1") && !pressed)
        {
            pressed = true;
        }
        if (Input.GetButtonUp("Fire1") && pressed)
        {
            pressed = false;
        }
        if (pressed)
        {
            if (!fire)
            {
                fire = true;
                time_shot = Time.time;
            }
            else
            {
                if (Time.time - time_shot > 0.4f && !cloud_perm)
                {
                    cloud_perm = true;
                    SpawnCloud();
                }
            }
        }
        if(fire && !pressed)
        {
            fire = false;
            if (cloud_perm)
            {
                cloud_perm = false;
                StartCoroutine(DestroyCloud());
            }
            else if (Time.time - last_shot > 0.5f)
            {
                last_shot = Time.time;
                StartCoroutine(SpawnEclair());

            }
        }
    }

    public GameObject cloud;
    public GameObject eclair;
    protected GameObject cloudy;
    protected Rigidbody2D m_cloudy;

    protected void SpawnCloud()
    {
        Vector2 posStart = m_Rigidbody.position + m_Capsule.offset + Vector2.left * (facingRight ? -1f : 1f) * m_Capsule.size.x  + new Vector2(0, 1);
        Vector2 speedStart= new Vector2 ((facingRight ? 1f : -1f)*4f, 0f);
        cloudy = Instantiate(cloud, posStart, new Quaternion(0,0,0,0));
        cloudy.SetActive(true); 
        m_cloudy = cloudy.GetComponent<Rigidbody2D>();
        m_cloudy.velocity = speedStart;
    }
    IEnumerator DestroyCloud()
    {
        Vector2 posStart =m_cloudy.position;
        GameObject bolt = Instantiate(eclair, posStart, new Quaternion(0, 0, 0, 0)); // Create a new game object from the prefab
        bolt.SetActive(true);
        Destroy(cloudy);
        // ensure the new object is Active to be visible within the scene
        yield return new WaitForSeconds(0.4f);
        Destroy(bolt);

    }
    IEnumerator SpawnEclair()
    {
        Vector2 posStart = m_Rigidbody.position + m_Capsule.offset + Vector2.left * (facingRight ? -1f : 1f) * m_Capsule.size.x  + new Vector2(0,1);
        GameObject bolt = Instantiate(eclair , posStart, new Quaternion(0,0,0,0)); // Create a new game object from the prefab
        bolt.SetActive(true); // ensure the new object is Active to be visible within the scene
        yield return new WaitForSeconds(0.4f);
        Destroy(bolt);
    }

}


