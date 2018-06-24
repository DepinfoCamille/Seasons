using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{

    public GameObject player1prefab;
    public GameObject player2prefab;
    public GameObject player3prefab;
    public GameObject player4prefab;

    GameObject player1, player2, player3, player4;


    private Component[] cams1;
    private Camera cam1;
    private Camera bgcam1;
    private Camera cam1prime;

    private Camera cam2;
    private Camera bgcam2;
    private Component[] cams2;

    private Camera cam3;
    private Camera bgcam3;
    private Component[] cams3;

    private Camera cam4;
    private Camera bgcam4;
    private Component[] cams4;


    // Use this for initialization
    void Start()
    {


        /*GameObject*/
        player1 = Instantiate(player1prefab) as GameObject;
        player1.SetActive(true); // ensure the new object is Active to be visible within the scene
        player1.GetComponent<PlayerControler>().SetPlayerNumber(1);

        if (PersistentDataExample.Instance.m_NbPlayers == 1)
        {
            cams1 = player1.GetComponentsInChildren(typeof(Camera));
            cam1 = (Camera)cams1[0];
            bgcam1 = (Camera)cams1[1];
            cam1.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
            //   cam1prime = Instantiate(cam1);
            //   cam1prime.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
            bgcam1.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
        }

        else if (PersistentDataExample.Instance.m_NbPlayers == 2)
        {
            /*GameObject*/
            player2 = Instantiate(player2prefab);
            player2.SetActive(true); // ensure the new object is Active to be visible within the scene
            player2.GetComponent<PlayerControler>().SetPlayerNumber(2);

            cams1 = player1.GetComponentsInChildren(typeof(Camera));
            cam1 = (Camera)cams1[0];
            bgcam1 = (Camera)cams1[1];
            cam1.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
            bgcam1.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);

            cams2 = player2.GetComponentsInChildren(typeof(Camera));
            cam2 = (Camera)cams2[0];
            bgcam2 = (Camera)cams2[1];
            cam2.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
            bgcam2.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);

        }
        else if (PersistentDataExample.Instance.m_NbPlayers == 3)
        {
            /*GameObject*/
            player2 = Instantiate(player2prefab);
            player2.SetActive(true); // ensure the new object is Active to be visible within the scene
            player2.GetComponent<PlayerControler>().SetPlayerNumber(2);

            /*GameObject*/
            player3 = Instantiate(player3prefab);
            player3.SetActive(true); // ensure the new object is Active to be visible within the scene
            player3.GetComponent<PlayerControler>().SetPlayerNumber(3);

            cams1 = player1.GetComponentsInChildren(typeof(Camera));
            cam1 = (Camera)cams1[0];
            bgcam1 = (Camera)cams1[1];
            cam1.rect = new Rect(0.0f, 0.5f, 0.5f, 1.0f);
            bgcam1.rect = new Rect(0.0f, 0.5f, 0.5f, 1.0f);
            bgcam1.rect = new Rect(0.0f, 0.5f, 0.5f, 1.0f);

            cams2 = player2.GetComponentsInChildren(typeof(Camera));
            cam2 = (Camera)cams2[0];
            bgcam2 = (Camera)cams2[1];
            cam2.rect = new Rect(0.0f, 0.0f, 0.5f, 0.0f);
            bgcam2.rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);

            cams3 = player3.GetComponentsInChildren(typeof(Camera));
            cam3 = (Camera)cams3[0];
            bgcam3 = (Camera)cams3[1];
            cam3.rect = new Rect(0.5f, 0.50f, 0.5f, 0.50f);
            bgcam3.rect = new Rect(0.5f, 0.50f, 0.5f, 0.50f);
        }
        else
        {
            player2 = Instantiate(player2prefab);
            player2.SetActive(true); // ensure the new object is Active to be visible within the scene
            player2.GetComponent<PlayerControler>().SetPlayerNumber(2);

            player3 = Instantiate(player3prefab);
            player3.SetActive(true); // ensure the new object is Active to be visible within the scene
            player3.GetComponent<PlayerControler>().SetPlayerNumber(3);

            player4 = Instantiate(player4prefab);
            player4.SetActive(true); // ensure the new object is Active to be visible within the scene
            player4.GetComponent<PlayerControler>().SetPlayerNumber(4);


            cams1 = player1.GetComponentsInChildren(typeof(Camera));
            cam1 = (Camera)cams1[0];
            bgcam1 = (Camera)cams1[1];
            cam1.rect = new Rect(0.0f, 0.5f, 0.5f, 1.0f);
            bgcam1.rect = new Rect(0.0f, 0.5f, 0.5f, 1.0f);
            bgcam1.rect = new Rect(0.0f, 0.5f, 0.5f, 1.0f);

            cams2 = player2.GetComponentsInChildren(typeof(Camera));
            cam2 = (Camera)cams2[0];
            bgcam2 = (Camera)cams2[1];
            cam2.rect = new Rect(0.0f, 0.0f, 0.5f, 0.0f);
            bgcam2.rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);

            cams3 = player3.GetComponentsInChildren(typeof(Camera));
            cam3 = (Camera)cams3[0];
            bgcam3 = (Camera)cams3[1];
            cam3.rect = new Rect(0.5f, 0.50f, 0.5f, 0.50f);
            bgcam3.rect = new Rect(0.5f, 0.50f, 0.5f, 0.50f);

            cams4 = player3.GetComponentsInChildren(typeof(Camera));
            cam4 = (Camera)cams3[0];
            bgcam4 = (Camera)cams3[1];
            cam4.rect = new Rect(0.5f, 0.00f, 0.5f, 0.5f);
            bgcam4.rect = new Rect(0.5f, 0.00f, 0.5f, 0.5f);

        }

        Debug.Log("Persistentdataexample " + PersistentDataExample.Instance.level0);

        if (/*PersistentDataExample.Instance.level0 == */true)
            // Je ne comprends pas pourquoi le PersistentDataExample.Instance.level0 est à false, ce n'est pas le cas dans ma version personnelle... 
        {
            Debug.Log("ok");
            Vector3 originTuto1 = new Vector3(10, 85, 0);
            Vector3 originTuto2 = new Vector3(10, 25, 0);
            Vector3 originTuto3 = new Vector3(230, 85, 0);
            Vector3 originTuto4 = new Vector3(230, 25, 0);

            player1.transform.Translate(originTuto1 - player1.transform.position);

            if (PersistentDataExample.Instance.m_NbPlayers >= 2)
            {
                player2.transform.Translate(originTuto2 - player2.transform.position);
            }

            if (PersistentDataExample.Instance.m_NbPlayers >= 3)
            {
                player3.transform.Translate(originTuto3 - player3.transform.position);
            }

            if (PersistentDataExample.Instance.m_NbPlayers == 4)
            {
                player4.transform.Translate(originTuto4 - player4.transform.position);
            }
        }


    }

    private void Update()
    {
        /*     if(PersistentDataExample.Instance.m_NbPlayers == 1)
             {
                 cam1prime = Instantiate(cam1);
                 cam1prime.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
             }*/
    }
}
