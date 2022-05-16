using System.Collections;
using Controllers.PlayerControls;
using UnityEngine;
using UnityEngine.UI;

namespace Sequences
{
    public class S1PlayerAwakening : MonoBehaviour
    {
        [SerializeField]
        private GameObject thePlayer;
    
        [SerializeField] 
        private GameObject fadeScreenIn;


        [SerializeField] 
        private GameObject playerAwakeningText;

        private void Start()
        {

            //Sets the fadein screen to active in code, as the canvas fills up my entire unity screen..
            fadeScreenIn.SetActive(true);
            
            //Disables the PlayerController at the start of the game, so the player can't walk around
            thePlayer.GetComponent<PlayerControls>().enabled = false;
            StartCoroutine(StartScenePlayer());
        }

        
        //Sets the text at the beginning of the game and removes it afterwards
        IEnumerator StartScenePlayer()
        {
            yield return new WaitForSeconds(1.5f);
            //test.enabled = false;
            fadeScreenIn.SetActive(false);
            playerAwakeningText.GetComponent<Text>().text = "What? Where am I?";
            yield return new WaitForSeconds(1.5f);
            playerAwakeningText.GetComponent<Text>().text = "";
            thePlayer.GetComponent<PlayerControls>().enabled = true;

        }
    }
}
