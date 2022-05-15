using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input_Manager_and_Camera.PlayerControls
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControls : MonoBehaviour
    {
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

        [SerializeField] private AudioSource walkingFx;

        //private GameObject _player;
        
        private CharacterController _controller;
        private bool _groundedPlayer;
        private Vector3 _playerVelocity;

        private Animator _animator;
        private String walkingString = "AnimatedWalking";
        private String jumpString = "Jump";
        private String crouchString = "Crouch";
        private String crouchWalkingString = "CrouchedAnimations";
        private float cameraCrouchHeight = 0.8f;
        private float cameraStandHeight = 1.6f;

        /*
        private Vector3 standingPlayer = new Vector3(1.6f, 1.6f, 1.6f);
        private Vector3 crouchingPlayer = new Vector3(1.6f, 0f, 1.6f);
        */

        private InputManager _inputManager;
        

        private void Start()
        {
            _inputManager = InputManager.Instance;

           // _player = GameObject.Find("Player");

        
            _controller = GetComponent<CharacterController>();
            
            _animator = _controller.GetComponent<Animator>();
            _animator.SetBool(crouchString, false);
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        private void Update()
        {
            #region PlayerMovement
            var x = _inputManager.GetPlayerMovement().x;
            var y = _inputManager.GetPlayerMovement().y;

            var move = new Vector3(x, 0, y);
            move = _controller.transform.rotation * move;
            move.y = 0f; //<-- Resets height

        
            if (move.magnitude > 1){
                move = move.normalized;
            }
        
            _controller.SimpleMove(move * playerSpeed);

            #endregion

            #region PlayerCamerea/ Rotation

            var mx = _inputManager.GetMouseDelta().x * Time.deltaTime;
            var my = - _inputManager.GetMouseDelta().y * Time.deltaTime;
        
            //Rotates character with mouse x value
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

            #endregion
            
            #region PlayerWalkingAnimation
            //If player is moving play sound/ setwalking string to 1 (playing walking animation) else, be idle
            if (_groundedPlayer && _controller.velocity.magnitude > 2f && walkingFx.isPlaying == false)
            {
                walkingFx.Play();
   
            }
            
            
            //If player is walking
            if (x != 0 || y != 0)
            {
                //If player is crouching
                if (_animator.GetBool(crouchString))
                {
                    _animator.SetFloat(crouchWalkingString, 1);
                }
                else
                {
                    _animator.SetFloat(walkingString, 1);
                }
   
            }
            //Else, if the player is standing still
            else if(x == 0 || y == 0)
            {
                //If the player is not crouching
                if (_animator.GetBool(crouchString) == false)
                {
                    _animator.SetFloat(walkingString, 0);
                }
                else
                {
                    _animator.SetFloat(crouchWalkingString, 0);
                    
                }
            }
            
            /*
            else if((x != 0 || y != 0) && _player.transform.localScale == crouchingPlayer)
            {
                //Debug.Log("CROUCH WALKING");
                //Debug.Log("X: " + x +"\n" + "Y: " + y );

                _animator.SetFloat(crouchWalkingString, 1);
            }
            else if((x == 0 || y == 0) && _player.transform.localScale == crouchingPlayer)
            {
                //Debug.Log("CROUCH IDLE ");
                //Debug.Log("X: " + x +"\n" + "Y: " + y );

                _animator.SetFloat(crouchWalkingString, 0);
            }
            */
            

            #endregion

            #region PlayerCrouching

            //Switched between standing and crouching
            if (_groundedPlayer && _inputManager.PlayerCrouch())
            {
                //Crouching
                if (_animator.GetBool(crouchString) == false)
                {
                    //Crouch position: X = 0, Y = 0.865, Z = 0.585
                    
                    //Changes the camerea position while crouching 
                    var cameraPosition = characterCamera.transform.position;
                    //cameraPosition.y -= 0.865f;
                    cameraPosition.y -= 0.5f;
                    //cameraPosition.z = 0.585f;
                    //cameraposition.x = 0f;
                    characterCamera.transform.position = cameraPosition;
                    _animator.SetBool(crouchString, true);
                }
                //Standing
                else
                {
                    //Standing position: X = 0, Y = 1.691, Z = 0.108
                    
                    //Resets camera position back to OG position
                    var cameraPosition = characterCamera.transform.position;
                    cameraPosition.y += 0.5f;
                    //cameraPosition.y += 0.865f;
                    //cameraPosition.z = 0.108f;
                    //cameraPosition.x = 0f;
                    characterCamera.transform.position = cameraPosition;
                    _animator.SetBool(crouchString, false);

                }
            }

            #endregion

            #region PlayerJumping
            
            //Checks if grounded or not
            _groundedPlayer = _controller.isGrounded;
            if (_groundedPlayer && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }
            
            // Changes the height position of the player.. Aka, Jumping
            if (_inputManager.PlayerJumpedThisFrame() && _groundedPlayer)
            {
                _animator.SetTrigger(jumpString);
                _playerVelocity.y += Mathf.Sqrt(jumpHeight * -6.0f * gravityValue);

            } 
            

            #endregion
            
            _playerVelocity.y += gravityValue * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);
            characterCamera.transform.localEulerAngles = new Vector3(currentRotationX, 0, 0);
        }

       
    }
}
