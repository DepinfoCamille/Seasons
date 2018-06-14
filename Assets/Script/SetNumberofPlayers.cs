using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNumberofPlayers : MonoBehaviour {

    public void Setup(int i)
    {
        PersistentDataExample.Instance.SetupNumberPlayers(i); 
        Debug.Log(message: PersistentDataExample.Instance.m_NbPlayers);
    }
}
