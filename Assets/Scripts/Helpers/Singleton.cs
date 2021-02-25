using UnityEngine;

namespace Helpers
{
    public class Singleton : MonoBehaviour
    {
        private static Singleton _singleton;
        // Start is called before the first frame update
        private void Awake()
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