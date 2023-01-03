using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Helicoopter
{
    public class EndMenu : MonoBehaviour
    {

        [SerializeField] LevelListSet levelList;
        public void BackMenu()
        {
            SceneManager.LoadScene("StartMenu");
        }

        public void RestartLevel()
        {

            SceneManager.LoadScene();
        }

        public void NextLevel()
        {
            SceneManager.LoadScene();
        }
    }
}
