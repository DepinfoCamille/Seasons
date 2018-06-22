using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    DeathScript deathzone;

    private void Start()
    {
        deathzone = GameObject.Find("DeathScript").GetComponent<DeathScript>();
    }
    void OnTriggerEnter (Collider other) {
        if(other.transform.gameObject.tag == "Player")
        {
            deathzone.respawnPositions = gameObject.transform.position;
            Destroy(gameObject);
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
