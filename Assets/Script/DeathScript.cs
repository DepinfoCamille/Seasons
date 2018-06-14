using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// another trap example, updating persistent data
// and reloading the current scene

public class DeathScript : MonoBehaviour {
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.CompareTag ("Player")) {
            PersistentDataExample.Instance.m_NbLiveUsed += 1;
            // access the persistent data through static public instance,
            // as the get operation we have defined in PersistentDataExample
            // will create the data if it does not already exist in the scene

			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
            // restart current scene
		}
	}
}
