using UnityEngine;

namespace UpsideDown.Player
{
    public class ResourcesManager : MonoBehaviour
    {
        public static ResourcesManager Instance;
        public GameResources playerResources = new(50, 50, 50);
        
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