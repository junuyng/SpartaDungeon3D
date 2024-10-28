using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonDontDestroy<T>: MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            if (instance == null)
            {
                GameObject SingletoneObject = new GameObject(nameof(T));
                instance = SingletoneObject.AddComponent<T>();
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        
        else if (instance != this)
        {
            Destroy(gameObject);  
        }
    }
}
