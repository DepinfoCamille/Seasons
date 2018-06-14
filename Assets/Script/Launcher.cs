using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour {
	public void StartGame(){
        // In order for a scene to be loaded it should be added to build settings:
        //   File > BuildSettings...
        SceneManager.LoadScene ("Niveau0");
        // All GameObjects that are not defined as 'DontDestroyOnLoad' will be lost (see PersistentDataExample)
	}
}
