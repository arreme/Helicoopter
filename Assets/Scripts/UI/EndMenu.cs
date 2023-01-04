using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        [SerializeField] private EventSystem _eventSystem;

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
            if (nextLevel != null)
                SceneManager.LoadScene(nextLevel.levelName);
            else
                SceneManager.LoadScene(3);
        }

        public void EndLevel(bool set)
        {
            repeatLevel = set;
            canvas.gameObject.SetActive(true);

            if (repeatLevel)
            {
                _eventSystem.firstSelectedGameObject = repeatButton.gameObject;
                repeatButton.gameObject.SetActive(true);
            }
            else
            {
                _eventSystem.firstSelectedGameObject = nextButton.gameObject;
                nextButton.gameObject.SetActive(false);
            }
        }
    }
}
