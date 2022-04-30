using UnityEngine;

namespace Input_Manager_and_Camera.PlayerControls
{
    [RequireComponent(typeof(CharacterController))] 
    public class PlayerController : MonoBehaviour
    {
        private InputManager _inputManager;
        
        #region Player speed and camera movement

        [SerializeField]
        private float playerSpeed = 2.0f;
        [SerializeField]
        private float jumpHeight = 1.0f;
        [SerializeField]
        private float gravityValue = -9.81f;
        
        private CharacterController _controller;
        private Vector3 _playerVelocity;
        private bool _groundedPlayer;

        private Transform cameraTransform;

        #endregion
        


        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _inputManager = InputManager.Instance;
            
            cameraTransform = Camera.main.transform;
        }

        //Fixed update?
        void Update()
        {
            #region Movement

                //Checks if grounded or not
                _groundedPlayer = _controller.isGrounded;
                if (_groundedPlayer && _playerVelocity.y < 0)
                {
                    _playerVelocity.y = 0f;
                }
                
                //Moves player
                Vector2 move = _inputManager.GetPlayerMovement();
                Vector3 movement = new Vector3(move.x, 0f, move.y);
                movement = cameraTransform.forward * movement.z + cameraTransform.right * movement.x;
                movement.y = 0f; //<-- Resets height
                _controller.Move(movement * (Time.deltaTime * playerSpeed));
                
                // Changes the height position of the player.. Aka, Jumping
                if (_inputManager.PlayerJumpedThisFrame() && _groundedPlayer)
                {
                    _playerVelocity.y += Mathf.Sqrt(jumpHeight * -6.0f * gravityValue);
                } 

                _playerVelocity.y += gravityValue * Time.deltaTime;
                _controller.Move(_playerVelocity * Time.deltaTime);

            #endregion



        }
    } 
}

