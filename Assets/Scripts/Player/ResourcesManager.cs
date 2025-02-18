using UnityEngine;

namespace UpsideDown.Player
{
    public class ResourcesManager : MonoBehaviour
    {
        public static ResourcesManager Instance;
        public GameResources playerResources;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}