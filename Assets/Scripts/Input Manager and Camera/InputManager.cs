using UnityEngine;
using UnityEngine.InputSystem;

namespace Input_Manager_and_Camera
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager _instance;
    

        //Singleton setup
        public static InputManager Instance => _instance;


        private MasterControls masterControls;
    
        private void Awake()
        {
            //More singleton setup
            
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
            
            masterControls = new MasterControls();
        
            //Hides the cursor and locks it within the game 
            //TODO: Test if it bugs out in the menu
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }

        #region ClassSetup

        private void OnEnable()
        {
            masterControls.Enable();
        }

        private void OnDisable()
        {
            masterControls.Disable();
        }

        #endregion


        #region PlayerActions

        public Vector2 GetPlayerMovement()
        {
            return masterControls.Player.Move.ReadValue<Vector2>();
        }
    
        public Vector2 GetMouseDelta()
        {
            return masterControls.Player.Look.ReadValue<Vector2>();
        }

        public bool PlayerJumpedThisFrame()
        {
            return masterControls.Player.Jump.triggered;

        }

        public bool InteractTriggered()
        {
            return masterControls.Player.Interact.triggered;
        }

        #endregion


    }
}