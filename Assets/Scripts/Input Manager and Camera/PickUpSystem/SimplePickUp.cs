using System;
using Input_Manager_and_Camera;
using Input_Manager_and_Camera.PickUpSystem;
using Input_Manager_and_Camera.PlayerControls;
using UnityEngine;
using UnityEngine.UI;

public class SimplePickUp : MonoBehaviour
{
    private InputManager _inputManager;

    [SerializeField]
    private Camera characterCamera;

    [SerializeField]
    private Transform slot;

    private PickableItem pickedItem;

    private float distanceToItem;
    
    //Sound stuff
    [SerializeField]
    private AudioSource pickUpItemFX;

    [SerializeField]
    private AudioSource dropItemFx;
    
    //UI Stuff
    [SerializeField]
    private GameObject onActionCross;
        
    [SerializeField]
    private GameObject interactKey;
        
    [SerializeField]
    private GameObject interactText;

    private void Start()
    {
        _inputManager = InputManager.Instance;

        distanceToItem = PlayerRayCasting.distanceFromTarget;

    }

    private void Update()
    {
        if (_inputManager.DropItemTriggered())
        {
            if (pickedItem)
            {
                DropItem(pickedItem);
                dropItemFx.Play();

            }
        }

        var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 2f))
        {
            // Check if object is pickable
            var pickable = hit.transform.GetComponent<PickableItem>();

            if (pickable && distanceToItem <= 2)
            {
                interactKey.SetActive(true);
                interactText.SetActive(true);
                onActionCross.SetActive(true);
                
                interactText.GetComponent<Text>().text = "Pick up " + pickable.name;

                if (_inputManager.InteractTriggered())
                {
                    if (pickedItem)
                    {
                        DropItem(pickedItem);
                        dropItemFx.Play();

                    }
                    else
                    {
                        pickUpItemFX.Play();
                        PickItem(pickable);
                    }
                    
                }

            }

        }
        else
        {
            interactKey.SetActive(false);
            interactText.SetActive(false);
            onActionCross.SetActive(false);
        }

/*
        
        if (_inputManager.DropItemTriggered() || _inputManager.InteractTriggered())
        {
            if (pickedItem)
            {
                DropItem(pickedItem);

            }
            
            else
            {
               // var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
               // RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 2f))
                {
                    // Check if object is pickable
                    var pickable = hit.transform.GetComponent<PickableItem>();
                    
                    if (pickable)
                    {
                        interactText.GetComponent<Text>().text = "Pick up " + pickable.name;

                        interactKey.SetActive(true);
                        interactText.SetActive(true);
                        onActionCross.SetActive(true);
                        
                        PickItem(pickable);

                    
                    }

                if (_inputManager.InteractTriggered())
                {
                    interactKey.SetActive(false);
                    interactText.SetActive(false);
                    onActionCross.SetActive(false);

                    PickItem(pickable);
                } 
                
               
                }
            }
        }*/

       
    }
    
    /*
    void OnMouseOver()
    {
        var ray = characterCamera.ViewportPointToRay(Vector3.one * 2f);

        // transform.position,transform.TransformDirection(Vector3.forward)
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f))
        {

            // Check if object is pickable
            var pickable = hit.transform.GetComponent<PickableItem>();

            if (pickable != null)
            {
                Debug.Log(pickable.name);

            }
            
            if (pickable)
            {
                Debug.Log("READY TO PICK " + pickable.name);

                //Debug
                interactText.GetComponent<Text>().text = "Pick up " + pickable.name;

                interactKey.SetActive(true);
                interactText.SetActive(true);
                onActionCross.SetActive(true);
                
            }
            
            if (pickable && _inputManager.InteractTriggered())
            {
                Debug.Log("PICKED");
                
                interactKey.SetActive(false);
                interactText.SetActive(false);
                

                // Pick it up
                PickItem(pickable);

            }
        }
    }

    
    void OnMouseExit()
    {
        interactKey.SetActive(false);
        interactText.SetActive(false);
        onActionCross.SetActive(false);
    }
    */
    
    private void PickItem(PickableItem item)
    {
        
        // Assign reference
        pickedItem = item;
            
        // Disable rigidbody and reset velocities
        item.Rb.isKinematic = true;
        item.Rb.velocity = Vector3.zero;
        item.Rb.angularVelocity = Vector3.zero;
            
        // Set Slot as a parent
        item.transform.SetParent(slot);
            
        // Reset position and rotation
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
    }

    
    private void DropItem(PickableItem item)
    {
       // if (!_inputManager.DropItemTriggered()) return;
        pickedItem = null;
        item.transform.SetParent(null);
        item.Rb.isKinematic = false;
        item.Rb.AddForce(item.transform.forward * 2, ForceMode.VelocityChange);

    }
}