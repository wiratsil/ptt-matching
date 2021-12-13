using UnityEngine;
using System.Linq;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType(typeof(T)) as T;

            //instantiate singleton
            if (instance == null)
            {
                var singletonObject = GameObject.Find("Singleton");
                if (!singletonObject)
                {
                    singletonObject = new GameObject();
                    singletonObject.name = "Singleton";//typeof(T).Name;
                }
                instance = singletonObject.AddComponent<T>();
            }

            return instance;
        }
    }

    void OnDestroy()
    {
        instance = GameObject.FindObjectsOfType(typeof(T)).Where((arg) => arg != instance) as T;
    }

    protected bool immortalOnlyObject()
    {
        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
            return true;
        }
        else
        {
            Destroy(gameObject);
            return false;
        }
    }
}
