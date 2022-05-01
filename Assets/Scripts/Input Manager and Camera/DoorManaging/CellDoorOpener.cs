using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Input_Manager_and_Camera.DoorManaging
{
    public class CellDoorOpener : MonoBehaviour
    {
        private InputManager _inputManager;

    
        [SerializeField]
        private float theDistance;

        [FormerlySerializedAs("actionDisplay")] [SerializeField]
        private GameObject interactKey;
        
        [FormerlySerializedAs("actionText")] [SerializeField]
        private GameObject interactText;
        
        [SerializeField]
        private GameObject theDoor;
        
        [SerializeField]
        private AudioSource openingSound;
        
        [SerializeField]
        private GameObject onActionCross;


        private void Start()
        {

            
            _inputManager = InputManager.Instance;
        }

        void Update()
        {

            theDistance = PlayerRayCasting.distanceFromTarget;

        }

        void OnMouseOver()
        {
            if (theDistance <= 2)
            {

                interactKey.SetActive(true);
                interactText.SetActive(true);
                onActionCross.SetActive(true);
            }

            if (_inputManager.InteractTriggered() && theDistance <= 2)
            {
                GetComponent<BoxCollider>().enabled = false;
                interactKey.SetActive(false);
                interactText.SetActive(false);
                theDoor.GetComponent<Animation>().Play("CellDoorOpen");
                openingSound.Play();

            }
        }

        void OnMouseExit()
        {
            interactKey.SetActive(false);
            interactText.SetActive(false);
            onActionCross.SetActive(false);

        }
    }
}
