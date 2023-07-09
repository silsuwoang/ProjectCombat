using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                var obj = FindObjectOfType<T>();
                // var obj = GameObject.Find(typeof(T).Name);
                if (obj)
                {
                    _instance = obj.GetComponent<T>();
                }
                else
                {
                    var newObj = new GameObject(typeof(T).Name);
                    _instance = newObj.AddComponent<T>();
                }
            }
            
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        _instance = GetComponent<T>();
        DontDestroyOnLoad(gameObject);
    }
}
