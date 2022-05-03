using UnityEngine;

namespace Input_Manager_and_Camera.PlayerControls
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControls : MonoBehaviour
    {
    

        // private CharacterController character;

        [SerializeField]
        private float playerSpeed = 5;

        [SerializeField]
        private float jumpHeight = 1.0f;
   
        [SerializeField]
        private float gravityValue = -9.81f;
    
        [SerializeField]
        private Camera characterCamera;

        [SerializeField]
        private float camSpeed = 0.5f;
    
        private CharacterController _controller;
        private bool _groundedPlayer;
        private Vector3 _playerVelocity;


    
        private InputManager _inputManager;
    
        // private MasterControls playerControls;

        // private Vector3 _startinRotation;

        /*
    private void Awake()
    {
        //_inputManager = new InputManager();
        //_inputManager = InputManager.Instance;

        //var x = _inputManager.GetPlayerMovement().x;

    }
*/

        private void Start()
        {
            _inputManager = InputManager.Instance;

            //playerControls = new MasterControls();

        
            _controller = GetComponent<CharacterController>();
        
            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
        /*
    #region InputSetup
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    

    #endregion
*/

    
        private void Update()
        {
            //Checks if grounded or not
            _groundedPlayer = _controller.isGrounded;
            if (_groundedPlayer && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }
        
            //Player movement
            var x = _inputManager.GetPlayerMovement().x;
            var y = _inputManager.GetPlayerMovement().y;
        
            var move = new Vector3(x, 0, y);
            move = _controller.transform.rotation * move;
            move.y = 0f; //<-- Resets height

        
            if (move.magnitude > 1)
                move = move.normalized;
        
            _controller.SimpleMove(move * playerSpeed);
        
            //Player camera
            var mx = _inputManager.GetMouseDelta().x * Time.deltaTime;
            var my = - _inputManager.GetMouseDelta().y * Time.deltaTime;
        
            _controller.transform.Rotate(Vector3.up, mx * camSpeed);
        
            var currentRotationX = characterCamera.transform.localEulerAngles.x;
            currentRotationX += my * camSpeed;

            if (currentRotationX < 180)
            {
                currentRotationX = Mathf.Min(currentRotationX, 60);
            }
            else if (currentRotationX > 180)
            {
                currentRotationX = Mathf.Max(currentRotationX, 300);
            }

            // Changes the height position of the player.. Aka, Jumping
            if (_inputManager.PlayerJumpedThisFrame() && _groundedPlayer)
            {
                _playerVelocity.y += Mathf.Sqrt(jumpHeight * -6.0f * gravityValue);
            } 
        
        
            //Player camera

            _playerVelocity.y += gravityValue * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);
            characterCamera.transform.localEulerAngles = new Vector3(currentRotationX, 0, 0);
        }

        /*

    #region PlayerMethods
    
    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }
    
    public bool PlayerJumpedThisFrame()
    {
        return playerControls.Player.Jump.triggered;

    }

    public bool InteractTriggered()
    {
        return playerControls.Player.Interact.triggered;
    }
    

    #endregion
    */
    }
}
