using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Input_Manager
{
    [RequireComponent(typeof(CharacterController))] 
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float playerSpeed = 2.0f;
        [SerializeField]
        private float jumpHeight = 1.0f;
        [SerializeField]
        private float gravityValue = -9.81f;
        
        private CharacterController _controller;
        private Vector3 _playerVelocity;
        private bool _groundedPlayer;

        private InputManager _inputManager;
        private Transform cameraTransform;
        

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _inputManager = InputManager.Instance;
            cameraTransform = Camera.main.transform;
        }

        void Update()
        {
            //Checks if grounded or not
            _groundedPlayer = _controller.isGrounded;
            if (_groundedPlayer && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }
            
            //Moves player
            //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector2 move = _inputManager.GetPlayerMovement();
            Vector3 movement = new Vector3(move.x, 0f, move.y);
            movement = cameraTransform.forward * movement.z + cameraTransform.right * movement.x;
            _controller.Move(movement * (Time.deltaTime * playerSpeed));

            //Checks if player is moving
            /*
            if (movement != Vector3.zero)
            {
                gameObject.transform.forward = movement;
            }
            */

            // Changes the height position of the player.. Aka, Jumping
            if (Input.GetButtonDown("Jump") && _groundedPlayer)
            {
                _playerVelocity.y += Mathf.Sqrt(jumpHeight * -6.0f * gravityValue);
            }

            _playerVelocity.y += gravityValue * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);
        }
    } 
}

