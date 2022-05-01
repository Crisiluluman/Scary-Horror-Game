using UnityEngine;

namespace Input_Manager_and_Camera.PickUpSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class PickableItem : MonoBehaviour
    {
        private Rigidbody rb;
        public Rigidbody Rb => rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
    }
}
