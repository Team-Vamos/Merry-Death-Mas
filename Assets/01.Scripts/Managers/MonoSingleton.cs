using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static bool _shuttingDown = false;
    private static object _locker = new object();
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_shuttingDown)
            {
                return null;
            }

            lock (_locker)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                        //DontDestroyOnLoad(instance);
                    }
                }
                return _instance;
            }

        }
    }
    private void Start()
    {
        _instance = null;
        _shuttingDown = false;
    }
   
    private void OnApplicationQuit()
    {
        _shuttingDown = true;
    }
}
