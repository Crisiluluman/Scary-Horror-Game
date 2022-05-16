using System;
using System.Collections;
using System.Collections.Generic;
using Controllers.PlayerControls;
using UnityEngine;
using UnityEngine.UI;

namespace Sequences
{
    public class S2ScreamTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject textbox;
        
        [SerializeField]
        private AudioSource scream;

        private void OnTriggerEnter(Collider other)
        {
           // _player.GetComponent<PlayerControls>().enabled = false;
            StartCoroutine(ScreamScenePlayer());
        }

        private IEnumerator ScreamScenePlayer()
        {
            scream.Play();//Takes a few milliseconds to start playing
            yield return new WaitForSeconds(0.8f);
            textbox.GetComponent<Text>().text = "What the fuck is that sound?!";
            yield return new WaitForSeconds(4f);
            textbox.GetComponent<Text>().text = "";
            //_player.GetComponent<PlayerControls>().enabled = true;

        }
    }
}
