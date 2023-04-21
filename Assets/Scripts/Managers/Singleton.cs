using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Deleting duplicate singleton...");
            Destroy(gameObject);
        }
    }
}
