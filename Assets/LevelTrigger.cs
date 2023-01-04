using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Helicoopter
{
    public class LevelTrigger : MonoBehaviour
    {
        [SerializeField] private string levelName;


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Attachable"))
            {
                if (levelName.Equals("Quit")) Application.Quit();
                else SceneManager.LoadScene(levelName);
            }
        }
    }
}
