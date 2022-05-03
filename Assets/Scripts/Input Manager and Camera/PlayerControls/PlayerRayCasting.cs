using UnityEngine;

namespace Input_Manager_and_Camera.PlayerControls
{
    public class PlayerRayCasting : MonoBehaviour
    {
    
        public static float distanceFromTarget;
        
        [SerializeField]
        private float toTarget;

        public static RaycastHit hit;


        void Update()
        {

           // RaycastHit hit;

            if (Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward), out hit))
            {
                //getHitOb = hit;
                toTarget = hit.distance;
                distanceFromTarget = toTarget;
            }

        }
    }
}
