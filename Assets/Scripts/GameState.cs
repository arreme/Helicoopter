using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Helicoopter
{
    public class GameState : MonoBehaviour
    {
        public static GameState Instance { get; private set; }

        public GameObject[] Players { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            Players = GameObject.FindGameObjectsWithTag("Player");
        }

        public void GameOver([CanBeNull] GameObject player = null)
        {
            Debug.Log("GAME OVER");
            
            
        }
    }
}