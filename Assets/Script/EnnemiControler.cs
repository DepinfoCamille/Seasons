using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnnemiControler : MonoBehaviour
{
	public float maxSpeed = 10f;	// max horizontal speed
	protected bool airControl = true;	// whehter control is allowed during jump phase
	public LayerMask groundLayers;	// layer used to test for the ground, should be defined through the inspector

	public float m_Scale=1;

	public GameObject explodePrefab;

	public float speed = 1.0f;

	public float health = 3.0f;
	private bool isInvincible = false ;
	private float healthStartTime;
	private float healthCooldown = 2.5f;

	// Current state of the player
	private bool grounded;	// true if player is considered grounded
	private bool facingRight = true;	// facing direction of the player


	public bool beginFacingLeft = false;

	public bool activation = false;
	private bool jump;	// save jump button status for fixed update

	// store Component of the Ennemi GameObject that need to be used in the script
	private Rigidbody2D m_Rigidbody;
	private CapsuleCollider2D m_Capsule;
	private Animator m_Animator;

	// HashId to manage the animation (faster than sting based approach)
	protected readonly int m_HashGroundedPara = Animator.StringToHash ("grounded");
	protected readonly int m_HashSpeedPara = Animator.StringToHash ("speed");
	protected readonly int m_HashVerticalSpeedPara = Animator.StringToHash ("vSpeed");
	protected readonly int m_HashRunPara = Animator.StringToHash ("run");
	protected readonly int m_HashDoFirePara = Animator.StringToHash ("doFire");

	private float startTime;
	public float jumpCooldown = 5f;
	public bool jumpActivation = false;


	private float startTimeB;
	public float fireCooldown = 5f;
	public bool fireActivation = false;
	private bool fire = false;

	// Contacts (we do not want to re-affects memory at every frame)
	public float groundedRaycastDistance = 0.1f;
	ContactFilter2D m_ContactFilter;
	RaycastHit2D[] m_HitBuffer = new RaycastHit2D[5];

	private void Awake ()
	{
		//Time initialization
		startTime = Time.time;
		startTimeB = Time.time;

		healthStartTime = Time.time;

		// get the component that will be used at each Update
		m_Animator = GetComponent<Animator> ();
		m_Rigidbody = GetComponent<Rigidbody2D> ();
		m_Capsule = GetComponent<CapsuleCollider2D> ();

		// define behavior for raycasting
		m_ContactFilter.layerMask = groundLayers;
		m_ContactFilter.useLayerMask = true;
		m_ContactFilter.useTriggers = false;
		Physics2D.queriesStartInColliders = false; // do not take into account collider within which we are starting the raycast
	
		if (beginFacingLeft) {
			Flip ();
			speed = -speed;
		}

	}



	void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.CompareTag ("KillerJump")) {




			var explode = Instantiate(explodePrefab, transform.position, Quaternion.identity);
			explode.SetActive(true);
			Destroy(explode, 0.3f); // we want to automatically destroy the explosion game object when the animation is finished
			Destroy (gameObject);
		}


		if (other.gameObject.CompareTag ("Damager") && !isInvincible) {

			health = health - 1f;
			isInvincible = true; 
			GetComponent<SpriteRenderer>().color = new Color(1f,1f - 1/(0.5f+health),1f - 1/(0.5f+ health));



			m_Rigidbody.velocity = new Vector2 (m_Rigidbody.velocity.x / 1.5f + maxSpeed / 1.5f, m_Rigidbody.velocity.y / 2 );
		}

		if (other.gameObject.CompareTag ("Player")) {


			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
			// restart current scene
		}

		if (other.gameObject.CompareTag ("MainCamera")) {
			//SceneManager.LoadScene ("EndlessFall");
			activation = true;
			m_Animator.SetBool ("run", true); 
		}
	
			

		if (other.gameObject.CompareTag ("Returner")) {
			speed=-speed;
		}

		if (other.gameObject.CompareTag ("Ennemi")) {
			//SceneManager.LoadScene ("EndlessFall");
			speed=-speed;
			// if scene are ordered, we can use the same script to transition
		}
	}



	// Update is called once per frame
	void Update ()
	{
		if (!jump && jumpActivation) {
			// Read the jump input in Update so button presses aren't missed.

			if (Time.time > startTime + jumpCooldown) {
				startTime = Time.time;
				jump = true;
			}
		}

		if (fireActivation) {
			fire = false;
			// Read the jump input in Update so button presses aren't missed.
			if (Time.time > startTimeB + fireCooldown) {
				startTimeB = Time.time;
				fire = true;
				m_Animator.SetBool (m_HashDoFirePara, false); 
			} else if(Time.time - startTimeB > fireCooldown - 1f) {
				m_Animator.SetBool (m_HashDoFirePara, true); 
			}
		


		if (isInvincible){
			if (Time.time > healthStartTime + healthCooldown) {
				healthStartTime = Time.time;
				isInvincible = false;
				GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f);
			
				//change size
				//Vector3 scale = new Vector3( 4f, 4f, 1f );
				//transform.localScale = scale;
			}
		}
		}
			

		if (health < 1f) {



			var explode = Instantiate(explodePrefab, transform.position, Quaternion.identity);
			explode.SetActive(true);
			Destroy(explode, 0.3f); // we want to automatically destroy the explosion game object when the animation is finished
			Destroy (gameObject);
		}

		CheckAndThrowBomb ();
	}

	// Fixed Update emulate constant time steps for physic engine
	private void FixedUpdate ()
	{
		// read continous inputs to obtain smooth motions

		if (activation) {
			//+ Random.Range(-1.0f,1.0f)

			float h = speed * m_DeltaTime;


			// check whether we are grounded or not, and update player status accordingly
			CheckIfGrounded ();

			// pass movement parameters to function that manage actual movement
			Move (h, jump);
			jump = false;

			// update the state of the animation state machine
			m_Animator.SetBool (m_HashGroundedPara, grounded); 


			m_Animator.SetFloat (m_HashSpeedPara, Mathf.Abs (h)); 

			// we handle both fall and jump animation based on the same animation 
			// that is parametrized by the vertical speed (see blend tree in Ennemi Animator)
			m_Animator.SetFloat (m_HashVerticalSpeedPara, m_Rigidbody.velocity.y);
		}

	}

	public void Move (float move, bool jump)
	{
		if (grounded || airControl) {
			m_Rigidbody.velocity = new Vector2 (move * maxSpeed, m_Rigidbody.velocity.y);

			// update facing of the player using the sprite to simplify the Animator
			if ((move > 0 && !facingRight) || (move < 0 && facingRight)) {
				Flip ();
			}


			if (grounded && jump) {
				grounded = false;
				m_Rigidbody.velocity = new Vector2 (m_Rigidbody.velocity.x / 1.5f, m_Rigidbody.velocity.y / 2 + maxSpeed / 1.5f);
			}
		}
	}

	private void Flip ()
	{
		facingRight = !facingRight;

		Vector3 s = transform.localScale;
		s.x *= -1;        
		transform.localScale = s;
		// can also be done using the flip attribut of the SpriteRenderer
	}


	public void CheckIfGrounded ()
	{
		// Compute raycast starting point and direction, such that the ray
		// is directed toward the ground and have a length of groundedRaycastDistance outside
		// the capsule collider of the character

		Vector2 raycastDirection = Vector2.down;
		Vector2 raycastStart = m_Rigidbody.position + m_Capsule.offset;
		raycastStart = raycastStart + Vector2.down * (m_Capsule.size.y * 0.5f - m_Capsule.size.x * 0.5f);
		float raycastDistance = m_Capsule.size.x * 2.5f/m_Scale + groundedRaycastDistance * 	2.5f/m_Scale;

		int count = Physics2D.Raycast(raycastStart, raycastDirection, m_ContactFilter, m_HitBuffer, raycastDistance);

		// We can check the ray that will be sent in the scene for debugging
		//Debug.DrawRay (raycastStart, raycastDirection); // * raycastDistance

		if (count > 0) {
			grounded = true;
		} else {
			grounded = false;
		}

		//m_HitBuffer[0] contains informations on the closest collider 

		//free memory
		for (int i = 0; i < m_HitBuffer.Length; i++) {
			m_HitBuffer [i] = new RaycastHit2D ();
		}
	}

	// Using a coroutine to fire bombs at fixed time steps when the fire button is pressed

	protected Coroutine m_ShootingCoroutine;
	protected float m_NextShotTime;
	protected float m_DeltaTime = 1f / 2f;
	protected bool m_HeldFire = false;

	protected IEnumerator Shoot ()
	{
		while (m_HeldFire) {
			if (Time.time >= m_NextShotTime) {
				SpawnBomb ();
				m_NextShotTime = Time.time + m_DeltaTime;
			}
			yield return null;
		}
	}

	// Launch corotines when the fire key is pressed
	// stop it when it is released
	public void CheckAndThrowBomb ()
	{
		if (fire) {
			if (m_ShootingCoroutine == null) {
				m_HeldFire = true;
				m_ShootingCoroutine = StartCoroutine (Shoot ());
				m_HeldFire = false;
				StopCoroutine (m_ShootingCoroutine);
				m_ShootingCoroutine = null;
			}
			
		}
	}

	public GameObject bombPrefab; // should be set from the inspector with a prefab containing the bomb GameObject
	// not that if a bomb GameObject is not already present in the scene the ressource should be loaded explicitly
	// this is the reason why there is a Bomb object in an inactive state within the scene
	protected float bulletSpeed = 10f;

	protected void SpawnBomb()
	{
		Debug.Log ("NewBomb");

		// Bomb object
		GameObject bomb = Instantiate(bombPrefab); // Create a new game object from the prefab
		bomb.SetActive(true); // ensure the new object is Active to be visible within the scene
		// Initialize the bomb velocity and position
		Vector2 posStart = m_Rigidbody.position + (m_Capsule.offset + Vector2.left * (facingRight ? -1.5f : 1.5f) * m_Capsule.size.x)*m_Scale;
		bomb.GetComponent<Rigidbody2D> ().position = posStart;
		bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(facingRight ? bulletSpeed/2 : -bulletSpeed/2, bulletSpeed/2);
	}

		


}
