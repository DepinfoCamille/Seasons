using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activation_levier : MonoBehaviour {
    bool activated;
    bool close;
    bool latence;
    public Sprite red;
    public Sprite green;
    SpriteRenderer image;
    int t;
    public GameObject mur = null;


    //bouger mur
    public bool can_move = false;

    // Use this for initialization
    void Start () {
        activated = false;
        close = false;
        latence = true;
        image = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        close = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        close = false;

    }
    // Update is called once per frame
    void Update () {
        t++;
		if ((Input.GetKey(KeyCode.E) || Input.GetButtonDown("Enter")) && close && latence)
        {
            activated = !activated;
            if(activated)
            {
                // pour déclencher une action, c'est ici!
                Action_Levier();
            }
            latence = false;
            t = 0;

        }
        if (t > 10) { latence = true; }
        if (activated)
        {
            image.sprite = green;
        }
        else
        {
            image.sprite = red;
        }
	}

    void Action_Levier()
    //ou ici aussi :)
    {
        can_move = true;
        if (mur != null)
        {
            //mur doit etre associé à wallControl
            mur.GetComponent<WallControl>().lever_mur();
        }
    }
}
