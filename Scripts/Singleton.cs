using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T _cache;
    private static System.Object _lock = new System.Object();
    private static bool _cached = false;

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_cached)
                    return _cache;

                _cache = FindObjectOfType<T>();

                if (_cache == null)
                {
                    GameObject gameObject = new GameObject($"[SINGLETON] {typeof(T)}");
                    _cache = gameObject.AddComponent<T>();
                    DontDestroyOnLoad(gameObject);
                }

                return _cache;
            }
        }
    }
}
