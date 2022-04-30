using System;
using UnityEngine;

namespace Input_Manager_and_Camera.DoorManaging
{
    public class CellDoorOpener : MonoBehaviour
    {
        private InputManager _inputManager;

    
        [SerializeField]
        private float theDistance;

        [SerializeField]
        private GameObject actionDisplay;
        
        [SerializeField]
        private GameObject actionText;
        
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

                actionDisplay.SetActive(true);
                actionText.SetActive(true);
                onActionCross.SetActive(true);
            }

            if (_inputManager.InteractTriggered() && theDistance <= 2)
            {
                GetComponent<BoxCollider>().enabled = false;
                actionDisplay.SetActive(false);
                actionText.SetActive(false);
                theDoor.GetComponent<Animation>().Play("CellDoorOpen");
                openingSound.Play();

            }
        }

        void OnMouseExit()
        {
            actionDisplay.SetActive(false);
            actionText.SetActive(false);
            onActionCross.SetActive(false);

        }
    }
}
