using System;
using System.Collections;
using Controllers;
using Controllers.PickUpSystem;
using Controllers.PlayerControls;
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

    [SerializeField]
    private GameObject subtitleText;
    private void Start()
    {
        _inputManager = InputManager.Instance;


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

            if (pickable )
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

    }

    private void PickItem(PickableItem item)
    {
        //May need better way for checking this
        switch (item.name)
        {
            case "Pistol":
                var gunScaleReset = new Vector3(0.2f,0.2f,0.2f);
                item.transform.localScale = gunScaleReset;
                StartCoroutine(ItemNotifier(item.name));
                break;
            case "Screwdriver":
                var screwdriverScaleReset = new Vector3(0.1f,0.1f,0.1f);
                item.transform.localScale = screwdriverScaleReset;
                StartCoroutine(ItemNotifier(item.name));
                break;
            default:
                var vanityScaleReset = new Vector3(2f,2f,2f);
                item.transform.localScale = vanityScaleReset;
                StartCoroutine(ItemNotifier(item.name));
                break;
        }
        
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
    
    
    //Probably also a better way to implement this..
    private IEnumerator ItemNotifier(String name)
    {
        switch (name)
        {
            case "Pistol":
                subtitleText.GetComponent<Text>().text = "A gun, this may come in handy";
                yield return new WaitForSeconds(1.5f);
                subtitleText.GetComponent<Text>().text = "";                
                break;
            case "Screwdriver":
                subtitleText.GetComponent<Text>().text = "Maybe I can use this on the door?";
                yield return new WaitForSeconds(1.5f);
                subtitleText.GetComponent<Text>().text = "";                
                break;
            default:
                subtitleText.GetComponent<Text>().text = "Hmm, not sure what use this for";
                yield return new WaitForSeconds(1.5f);
                subtitleText.GetComponent<Text>().text = "";                
                break;
        }    }


}