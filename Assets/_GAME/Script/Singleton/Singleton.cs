using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static object _lock = new object();

    private static bool _isApplicationQuit = false;

    public static T Instance
    {
        get
        {
            if (_isApplicationQuit)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "[Singleton] " + typeof(T);
                        DontDestroyOnLoad(singleton);
                    }
                }

                return _instance;
            }
        }
    }

    public virtual void Init()
    {
        DontDestroyOnLoad(gameObject);
        Reset();
    }
    
    public virtual void Reset()
    {
        _isApplicationQuit = false;
    }

    public void OnDestroy()
    {
        _isApplicationQuit = true;
    }

    void OnApplicationQuit()
    {
        _isApplicationQuit = true;
    }
}