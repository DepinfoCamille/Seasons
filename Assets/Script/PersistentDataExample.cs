using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// exemple to keep data between scene

public class PersistentDataExample : MonoBehaviour
{
    public int m_NbLiveUsed = 0;
    public int m_NbPlayers = 1;
    public bool level0 = true;

    // 
    public static PersistentDataExample Instance
    {
        get
        {
            // if static variable is already initialized, use it
            if (s_Instance != null)
                return s_Instance;

            // otherwise find if it is already present in the scene
            s_Instance = FindObjectOfType<PersistentDataExample>();
            if (s_Instance != null)
                return s_Instance;

            // if not found create it
            Create();

            return s_Instance;
        }
    }

    protected static PersistentDataExample s_Instance;

    public void SetupNumberPlayers(int i)
    {
        m_NbPlayers = i;
    }

    public static PersistentDataExample Create()
    {
        // Create a new empty game object ot store the script
        GameObject persistentDataGameObject = new GameObject("PersistentDataExample");
        DontDestroyOnLoad(persistentDataGameObject); // GameObject (and associated script) should not be destroyed between scene
        s_Instance = persistentDataGameObject.AddComponent<PersistentDataExample>();
        return s_Instance;
    }

    void Awake()
    {
        if (Instance != this)
        {
            // if the static instence does not correspond to this object, destroy it to avoid duplicates
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // GameObject (and associated script) should not be destroyed between scene
    }
}
