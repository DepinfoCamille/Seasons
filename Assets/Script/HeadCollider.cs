using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollider : MonoBehaviour {


	public Transform davodaParent;

	private float maxSpeed = 11f;
	private Rigidbody2D m_Rigidbody;

	Vector3 v;


	void Update (){
		davodaParent = transform.parent;
		m_Rigidbody = davodaParent.GetComponent<Rigidbody2D>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		if (other.gameObject.CompareTag ("Ennemi")) {

			m_Rigidbody.velocity = new Vector2 (m_Rigidbody.velocity.x / 1.5f, maxSpeed/1.3f);

			//other.transform.parent.rigidbody.AddForce(v * Time.deltaTime,ForceMode2D);
		}

	}
}
