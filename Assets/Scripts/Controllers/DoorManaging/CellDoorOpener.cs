using System;
using System.Collections;
using Controllers.PickUpSystem;
using Controllers;
using Controllers.PlayerControls;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.DoorManaging
{
    public class CellDoorOpener : MonoBehaviour
    {
        private InputManager _inputManager;

        [SerializeField] 
        private GameObject grabslot;
        
        [SerializeField] 
        private GameObject screwdriver;
        
        [SerializeField] 
        private GameObject textbox;
    
        private float theDistance;

        //[FormerlySerializedAs("actionDisplay")] 
        [SerializeField]
        private GameObject interactKey;
        
        //[FormerlySerializedAs("actionText")] 
        [SerializeField]
        private GameObject interactText;
        
        [SerializeField]
        private GameObject theDoor;
        
        [SerializeField]
        private AudioSource lockedDoorSound;
        
        [SerializeField]
        private AudioSource openingSound;
        
        [SerializeField]
        private AudioSource lockPickSound;

        
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
                //Debug.Log(interactText.GetComponent<Text>().text);

            
                interactText.GetComponent<Text>().text = "Open door";
                
                interactKey.SetActive(true);
                interactText.SetActive(true);
                onActionCross.SetActive(true);

                if (_inputManager.InteractTriggered())
                {
                    if (screwdriver.transform.IsChildOf(grabslot.transform))
                    {
                        StartCoroutine( StartLockPick());
                    
                    }
                    else
                    {
                        //Suspends routine to so I can show the text for a few seconds
                        StartCoroutine(StartLockedTextbox());
                    }

                }
            }

           
        }

        IEnumerator StartLockedTextbox()
        {
            lockedDoorSound.Play();
            textbox.SetActive(true);
            textbox.GetComponent<Text>().text = "Locked.. Maybe I can pick the lock?";
            yield return new WaitForSeconds(2.5f);
            textbox.GetComponent<Text>().text = "";
            textbox.SetActive(false); 
        }
        
        IEnumerator StartLockPick()
        {

            lockPickSound.Play();
            yield return new WaitForSeconds(1.5f);
            
            GetComponent<BoxCollider>().enabled = false;
            interactKey.SetActive(false);
            interactText.SetActive(false);
            theDoor.GetComponent<Animation>().Play("CellDoorOpen");
            openingSound.Play();

            
            textbox.GetComponent<Text>().text = "Nice, now where was that pistol?";
            Debug.Log(textbox.GetComponent<Text>().text);

            yield return new WaitForSeconds(2f);
            textbox.GetComponent<Text>().text = "";
            
            Debug.Log(textbox.GetComponent<Text>().text);



        }
        
        
        void OnMouseExit()
        {
            interactKey.SetActive(false);
            interactText.SetActive(false);
            onActionCross.SetActive(false);

        }
    }
}
