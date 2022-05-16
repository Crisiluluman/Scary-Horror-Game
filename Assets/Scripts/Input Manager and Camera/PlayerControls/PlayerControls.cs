using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input_Manager_and_Camera.PlayerControls
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControls : MonoBehaviour
    {
        //Stuff to set in unity
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

        //Checks for jumping
        private bool groundedPlayer;
        private Vector3 playerVelocity;
        
        //Animation stuff and Strings, incase I rename them later
        private Animator animator;
        private String walkingString = "AnimatedWalking";
        private String jumpString = "Jump";
        private String crouchString = "Crouch";
        private String crouchWalkingString = "CrouchedAnimations";

        
        private CharacterController _controller;
        private InputManager _inputManager;
        

        private void Start()
        {
            _inputManager = InputManager.Instance;
            _controller = GetComponent<CharacterController>();
            
            //Initalize crouching with false
            animator = _controller.GetComponent<Animator>();
            animator.SetBool(crouchString, false);
            
            //Invisible and locked mouse while playing
            //Should change when the pause menu is open
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
            if (groundedPlayer && _controller.velocity.magnitude > 2f && walkingFx.isPlaying == false)
            {
                walkingFx.Play();
   
            }
            
            
            //If player is walking
            if (x != 0 || y != 0)
            {
                //If player is crouching
                if (animator.GetBool(crouchString))
                {
                    animator.SetFloat(crouchWalkingString, 1);
                }
                else
                {
                    animator.SetFloat(walkingString, 1);
                }
   
            }
            //Else, if the player is standing still
            else if(x == 0 || y == 0)
            {
                //If the player is not crouching
                if (animator.GetBool(crouchString) == false)
                {
                    animator.SetFloat(walkingString, 0);
                }
                else
                {
                    animator.SetFloat(crouchWalkingString, 0);
                    
                }
            }
            
            #endregion

            #region PlayerCrouching

            //Switched between standing and crouching
            if (groundedPlayer && _inputManager.PlayerCrouch())
            {
                //Crouching
                if (animator.GetBool(crouchString) == false)
                {
                    
                    //Changes the camerea position while crouching 
                    var cameraPosition = characterCamera.transform.position;
                    cameraPosition.y -= 0.5f;
                    //Unable to change z value (Thats why you can see the player while crouching)
                    //Its not a bug, its a feature
                    characterCamera.transform.position = cameraPosition;
                    animator.SetBool(crouchString, true);
                }
                //Standing
                else
                {
                    //Resets camera position back to OG position
                    var cameraPosition = characterCamera.transform.position;
                    cameraPosition.y += 0.5f;
                    characterCamera.transform.position = cameraPosition;
                    animator.SetBool(crouchString, false);

                }
            }

            #endregion

            #region PlayerJumping
            
            //Checks if grounded or not
            groundedPlayer = _controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }
            
            // Changes the height position of the player.. Aka, Jumping
            if (_inputManager.PlayerJumpedThisFrame() && groundedPlayer)
            {
                animator.SetTrigger(jumpString);
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -6.0f * gravityValue);

            } 
            

            #endregion
            
            playerVelocity.y += gravityValue * Time.deltaTime;
            _controller.Move(playerVelocity * Time.deltaTime);
            characterCamera.transform.localEulerAngles = new Vector3(currentRotationX, 0, 0);
        }

       
    }
}
