using UnityEngine;

namespace UpsideDown.Player
{
    // PlayerInputManager is a singleton class that stores our InputActions object
    // as a public field. This allows us to access the InputActions object from
    // any script which may require reading input from the player.
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;
        public InputActions PlayerInputActions;
        
        private void Awake()
        {
            if (Instance == null)
            {
                PlayerInputActions = new InputActions();
                PlayerInputActions.Enable();
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
