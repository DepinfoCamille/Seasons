using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyBehavior : MonoBehaviour {

	float timeCounter;
	PlayerControler PC;
	bool facingRight;

	// Use this for initialization
	void Start () {
		timeCounter = 0;
		PC = GetComponentInParent<PlayerControler> ();
		facingRight = PC.facingRight;
	}

	// Update is called once per frame
	void Update () {
		facingRight = PC.facingRight;
		Debug.Log (facingRight);
		timeCounter += Time.deltaTime;
					
		float x = Mathf.Cos (timeCounter);
		float y = Mathf.Sin (timeCounter);
		float z = -0.5f;

		if (facingRight) {
			transform.localPosition = new Vector3 (x, y, z);
		} else {
			transform.localPosition = new Vector3 (-x, y, z);
		}

	}
}
