using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cube_Movement : MonoBehaviour
{
    public Rigidbody rb;

    public float moveSpeed = 50f;
    public InputAction playerControls;
    private Vector3 moveDirection = Vector3.zero;

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);
    }
    
    void FixedUpdate()
    {
        moveDirection = playerControls.ReadValue<Vector3>();
    }
}
