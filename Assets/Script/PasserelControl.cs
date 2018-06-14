using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// example of coroutines usage to animate an element of the scenery
// without using an update function that would be called at every frame

public class PasserelControl : MonoBehaviour 
{
	public float smoothing = 0.4f;
	public Transform target;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			StartCoroutine(MovePasserel(target));
		}
	}

	IEnumerator MovePasserel (Transform target)
	{
		float t = 0;
		Vector3 orig = transform.position; 
        // if we do not store the origin, the movement do not have constant speed

		while(Vector3.Distance(orig, target.position) > 0.05f)
		{ // continue until we reached the destination

            //t += smoothing * Time.deltaTime;
            //transform.position = Vector3.Lerp(orig, target.position, t / 5f);
            //yield return null; 

            // we animate the "passerel" with the physic engine because we use physic for the player
            // use above alternative to see artefacts due to collision managements
			t += smoothing * Time.fixedDeltaTime;
			transform.position = Vector3.Lerp(orig, target.position, t / 5f); // Lerp is interpolating between two vectors
			yield return new WaitForFixedUpdate ();
		}

		print("Reached the target.");
		yield return new WaitForSeconds(3f); // just an exemple...
		print("MyCoroutine is now finished.");
	}

}
