using System.Collections;
using Cinemachine;
using Controllers;
using UnityEngine;
using UnityEngine.UI;


namespace Controllers.PistolControls
{
    public class PistolControls : MonoBehaviour
    {
        [SerializeField] private GameObject pistol;
        [SerializeField] private GameObject grabslot;
        [SerializeField] private AudioSource pistolShot;
        [SerializeField] private GameObject subtitleText;
        [SerializeField] private AudioSource emptyPistol;

        private float bullets;
        [SerializeField] private float damage;
        [SerializeField] private float range;
        [SerializeField] private Camera fpsCam;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private GameObject bulletImpact;


        //private Transform pistolBolt;

        private InputManager _inputManager;

        void Start()
        {
            _inputManager = InputManager.Instance;

            //8 bullets
            bullets = 100f;
            
            
            pistol = GameObject.Find("Pistol");
            //pistolBolt = pistol.transform.GetChild(0);
        }

        void Update()
        {
            if (pistol.transform.IsChildOf(grabslot.transform) && _inputManager.FireTriggered())
            {
                bullets -= 1;
                if (bullets <= 0)
                {
                    StartCoroutine(StartOutOfAmmo());
                }
                else
                {
                    pistolShot.Play();
                    pistol.GetComponent<Animation>().Play("PistolBlowback");
                
                    Shoot();
                }

            }
        }

        private void Shoot()
        {
            muzzleFlash.Play();
            
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log("Pew pew the " + hit.transform.name);

                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }

                //Displays the particle effect on so the rotation matches outward of surface hit
                GameObject impact = Instantiate(bulletImpact, hit.point,Quaternion.LookRotation(hit.normal));
                Destroy(impact,1.5f); //Destroys object after 1.5 seconds
            }
        }

        private IEnumerator StartOutOfAmmo()
        {            
            emptyPistol.Play();
            subtitleText.GetComponent<Text>().text = "Out of ammo..";
            yield return new WaitForSeconds(0.8f);
            subtitleText.GetComponent<Text>().text = "";        }
    }
}
