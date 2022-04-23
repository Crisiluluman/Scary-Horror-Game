using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance => _instance;


    private MasterControls masterControls;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        masterControls = new MasterControls();
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        masterControls.Enable();
    }

    private void OnDisable()
    {
        masterControls.Disable();
    }

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
}

