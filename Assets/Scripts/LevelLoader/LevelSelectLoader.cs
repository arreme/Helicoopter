using System;
using System.Collections;
using System.Collections.Generic;
using Helicoopter.LevelLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Helicoopter
{
    public class LevelSelectLoader : MonoBehaviour
    {
        [SerializeField] private LevelListSet levelListSet;
        
        private void Awake()
        {
            int maxLevels = levelListSet.Items.Count;
            for (int i = 0; i < maxLevels; i++)
            {
                Transform t = transform.GetChild(i);
                Levels current = levelListSet.Items[i];
                
                if (current.isLocked)
                {
                    t.GetChild(2).gameObject.SetActive(true);
                    t.GetChild(1).gameObject.SetActive(false);
                    t.GetChild(3).gameObject.SetActive(false);
                    t.GetChild(4).gameObject.SetActive(false);
                }
                else if( current.isBeaten)
                {
                    t.GetChild(0).gameObject.SetActive(true);
                    t.GetChild(1).gameObject.SetActive(false);
                }
            }
        }

        public void SetLevel(int level)
        {
            SceneManager.LoadScene(levelListSet.Items[level].levelName);
        }
    }
}
