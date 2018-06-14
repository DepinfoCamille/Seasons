using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

	public float timer = 1000f;
    public GameObject explodePrefab;

	void OnEnable()
	{
		StartCoroutine (Fire());
	}

	IEnumerator Fire()
	{
		float ttt = Time.time;
		yield return new WaitForSeconds(3.0f);

        // We can check that the time is only at least 3 seconds
        Debug.Log ("TIME :");
		Debug.Log (Time.time-ttt);

        // same principle as the creation of bomb in player
        var explode = Instantiate(explodePrefab, transform.position, Quaternion.identity);
        explode.SetActive(true);
        Destroy(explode, 0.3f); // we want to automatically destroy the explosion game object when the animation is finished

		Destroy(gameObject);
	}
}
