using UnityEngine;

namespace Input_Manager_and_Camera.PlayerControls
{
    public class PlayerRayCasting : MonoBehaviour
    {
    
        public static float distanceFromTarget;
        [SerializeField]
        private float toTarget;


        void Update()
        {

            RaycastHit Hit;

            if (Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward), out Hit))
            {
                toTarget = Hit.distance;
                distanceFromTarget = toTarget;
            }

        }
    }
}
