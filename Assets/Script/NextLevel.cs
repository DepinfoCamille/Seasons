using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
            if (SceneManager.GetActiveScene().name == "Plateforms")
            {
                SceneManager.LoadScene("Niveau1");
            }
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            // if scene are ordered, we can use the same script to transition
		}
	}
}
