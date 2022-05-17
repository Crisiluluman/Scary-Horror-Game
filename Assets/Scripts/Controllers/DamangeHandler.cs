using System;
using UnityEngine;

namespace Controllers
{
    public class DamangeHandler : MonoBehaviour
    {
        [SerializeField] private float health = 50f;

        [SerializeField] private GameOverScreen _gameOverScreen;

        private Animator animator;
        private String mutanDeathString = "MutantDeath";
        private String playerDeathString = "PlayerDeath";


        public void TakeDamage(float amount, String target)
        {
            health -= amount;

            //Debug.Log("HEALTH: " + health );

            if (health <= 0f)
            {
                Die(target);
            }
        }

        private void Die(String target)
        {
            if (String.Equals(target,"Mutant"))
            {
                var enemyToDie = GameObject.Find(target);
                animator = enemyToDie.GetComponent<Animator>();
                animator.SetBool(mutanDeathString, true);
                Destroy(gameObject,2f);
            
                var player = GameObject.Find("Player");
                player.GetComponent<PlayerControls.PlayerControls>().enabled = false;

                RunGameOver();

            }
            else if (String.Equals(target,"Player"))
            {
                var playerToDie = GameObject.Find(target);
                animator = playerToDie.GetComponent<Animator>();
                animator.SetBool(playerDeathString, true);
                playerToDie.GetComponent<PlayerControls.PlayerControls>().enabled = false;
            
                RunGameOver();

            }
        }

        private void RunGameOver()
        {
            Cursor.lockState = CursorLockMode.None;;
            Cursor.visible = true;
            _gameOverScreen.Setup();
        }
    }
}
