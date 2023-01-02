using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Helicoopter
{
    public class StartMenu : MonoBehaviour
    {
        public void QuitApplication()
        {
            Application.Quit(0);
        }

        public void PlayGame()
        {
            SceneManager.LoadScene("PlayerSelectionMenu");
        }
    }
}
