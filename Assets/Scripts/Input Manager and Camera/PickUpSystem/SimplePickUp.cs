
using Input_Manager_and_Camera.PlayerControls;
using UnityEngine;
using UnityEngine.UI;

namespace Input_Manager_and_Camera.PickUpSystem
{
    public class SimplePickUp : MonoBehaviour
    {
        private InputManager _inputManager;
        
        //Checks if player already has item
        private PickableItem pickedItem;

        [SerializeField]
        private float theDistance;
        
        //[SerializeField] private GameObject playerCamera; //The Camera that follows the player

        [SerializeField]
        private Transform itemSlot;

        [SerializeField]
        private GameObject thePistol;
        
        [SerializeField]
        private GameObject onActionCross;
        
        [SerializeField]
        private GameObject interactKey;
        
        [SerializeField]
        private GameObject interactText;
        
        
        private void Start()
        {
            _inputManager = InputManager.Instance;

            //_inputManager = new InputManager();
        }

        private void Update()
        {
            theDistance = PlayerRayCasting.distanceFromTarget;

        }

        void OnMouseOver()
        {

            if (theDistance <= 2)
            {
                
                if (pickedItem)
                {
                    DropItem(pickedItem);
                }
                
                //Debug
                interactText.GetComponent<Text>().text = "Pick up gun";

                interactKey.SetActive(true);
                interactText.SetActive(true);
                onActionCross.SetActive(true);
                
            }
            
            if (_inputManager.InteractTriggered() && theDistance <= 2)
            {
                interactKey.SetActive(false);
                interactText.SetActive(false);
                
                var pickable = thePistol.transform.GetComponent<PickableItem>();
                        
                // If object has PickableItem class
                if (pickable)
                {
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


        public void PickItem(PickableItem item)
        {
            
            Debug.Log("HELOOO" + item.name);
            
            // Assign reference
            pickedItem = item;
            
            // Disable rigidbody and reset velocities
            item.Rb.isKinematic = true;
            item.Rb.velocity = Vector3.zero;
            item.Rb.angularVelocity = Vector3.zero;
            
            // Set Slot as a parent
            item.transform.SetParent(itemSlot);
            
            // Reset position and rotation
            item.transform.localPosition = Vector3.zero;
            item.transform.localEulerAngles = Vector3.zero;
        }
        
        private void DropItem(PickableItem item)
        {
            //TODO: Fix
            // Remove reference
            pickedItem = null;
            // Remove parent
            item.transform.SetParent(null);
            // Enable rigidbody
            item.Rb.isKinematic = false;
            // Add force to throw item a little bit
            item.Rb.AddForce(item.transform.forward * 2, ForceMode.VelocityChange);
        }
    }
}