using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Helicoopter
{
    public class EndMenu : MonoBehaviour
    {

        [SerializeField] Levels actLevel;
        [SerializeField] Levels nextLevel;
        [SerializeField] Canvas canvas;
        [SerializeField] Button repeatButton;
        [SerializeField] Button nextButton;
        private bool repeatLevel = false;

        public void Awake()
        {
            canvas.gameObject.SetActive(false);
            repeatButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
        }
        public void BackMenu()
        {
            SceneManager.LoadScene("StartMenu");
        }

        public void RestartLevel()
        {

            SceneManager.LoadScene(actLevel.levelName);
        }

        public void NextLevel()
        {
            SceneManager.LoadScene(nextLevel.levelName);
        }

        public void EndLevel(bool set)
        {
            repeatLevel = set;
            canvas.gameObject.SetActive(true);

            if (repeatLevel) repeatButton.gameObject.SetActive(true);
            else nextButton.gameObject.SetActive(false);
        }
    }
}
