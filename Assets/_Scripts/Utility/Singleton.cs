using UnityEngine;

namespace BossRush.Utility
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = RetrieveSingleton();
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            // delete other found
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }

            _instance = this as T;
            DontDestroyOnLoad(gameObject);
            Debug.Log($"[Singleton] {typeof(T).Name} initialized as {gameObject.name}");
        }

        private static T RetrieveSingleton()
        {
            _instance = FindFirstObjectByType<T>();
            if ( _instance != null )
            {
                return _instance;
            }

            var singletonObj = new GameObject(typeof(T).Name);
            var newInstance = singletonObj.AddComponent<T>();
            return newInstance;
        }
    }
}
