using System.Collections;
using UnityEngine;

namespace Light
{
    public class FlickerControl : MonoBehaviour
    {
        [SerializeField] private float minFlicker = 0.01f;
        [SerializeField] private float maxFlicker = 0.2f;

        public bool isFlickering = false;
        public float timeDelay;
        void Update()
        {
            if (isFlickering == false)
            {
                StartCoroutine(FlickeringLight());
            }
        }

        IEnumerator FlickeringLight() {
            isFlickering = true;

            //Randomizes amount of flickeirng 
            this.gameObject.GetComponent<UnityEngine.Light>().enabled = false;
            timeDelay = Random.Range(minFlicker, maxFlicker);

            //Returns the flicker value
            yield return new WaitForSeconds(timeDelay);
            this.gameObject.GetComponent<UnityEngine.Light>().enabled = true;
        
            //Same as before, but adds another layer of randomness
            yield return new WaitForSeconds(timeDelay);
            this.gameObject.GetComponent<UnityEngine.Light>().enabled = true;

            isFlickering = false;
        }


    }
}
