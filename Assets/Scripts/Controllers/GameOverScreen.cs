using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class GameOverScreen : MonoBehaviour
    {

        public void Setup()
        {
            gameObject.SetActive(true);   
        }

        public void onExit()
        {
            gameObject.SetActive(false);   

        }

        public void Restart()
        {
            SceneManager.LoadScene("SampleScene");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
