using System.Collections;
using UnityEngine;

namespace Input_Manager_and_Camera.PistolControls
{
    public class PistolControls : MonoBehaviour
    {
        [SerializeField] private GameObject pistol;
        [SerializeField] private GameObject grabslot;
        [SerializeField] private AudioSource pistolShot;

        //private Transform pistolBolt;

        private InputManager _inputManager;

        void Start()
        {
            _inputManager = InputManager.Instance;
            
            pistol = GameObject.Find("Pistol");
            //pistolBolt = pistol.transform.GetChild(0);
        }

        void Update()
        {
            if (pistol.transform.IsChildOf(grabslot.transform) && _inputManager.FireTriggered())
            {
                pistolShot.Play();
                //Debug.Log(pistolBolt.name);
                //pistolBolt.GetComponent<Animation>().Play("PistolBlowback");
                pistol.GetComponent<Animation>().Play("PistolBlowback");
                
                Debug.Log("Pew pew");

            }
        }


    }
}
