using UnityEngine;
using UnityEngine.InputSystem;

namespace Input_Manager_and_Camera
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager _instance;
    

        //Singleton setup
        public static InputManager Instance => _instance;


        private MasterControls _masterControls;
    
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
            
            _masterControls = new MasterControls();
        

            

        }

        #region ClassSetup

        private void OnEnable()
        {
            _masterControls.Enable();
        }

        private void OnDisable()
        {
            _masterControls.Disable();
        }

        #endregion


        #region PlayerActions

        public Vector2 GetPlayerMovement()
        {
            return _masterControls.Player.Move.ReadValue<Vector2>();
        }
    
        public Vector2 GetMouseDelta()
        {
            return _masterControls.Player.Look.ReadValue<Vector2>();
        }

        public bool PlayerJumpedThisFrame()
        {
            return _masterControls.Player.Jump.triggered;
        }
        
        public bool PlayerCrouch()
        {
            return _masterControls.Player.Crouch.triggered;
        }

        public bool InteractTriggered()
        {
            return _masterControls.Player.Interact.triggered;
        }
        
        public bool DropItemTriggered()
        {
            return _masterControls.Player.DropItem.triggered;
        }

        public bool FireTriggered()
        {
            return _masterControls.Player.Fire.triggered;
        }
        
        #endregion


    }
}