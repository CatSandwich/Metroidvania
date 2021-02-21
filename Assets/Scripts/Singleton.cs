using UnityEngine;

namespace Assets.Scripts
{
    public class Singleton : MonoBehaviour
    {
        private static Singleton _singleton;
        // Start is called before the first frame update
        void Awake()
        {
            if (_singleton != null)
            {
                Destroy(gameObject);
                return;
            }

            _singleton = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
