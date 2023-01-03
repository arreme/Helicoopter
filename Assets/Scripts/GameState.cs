using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Helicoopter
{
    public class GameState : MonoBehaviour
    {
        public static GameState Instance { get; private set; }

        [HideInInspector] public GameObject[] Players { get; private set; }
        private CameraController _cameraController;

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
            if (Camera.main != null) _cameraController = Camera.main.gameObject.GetComponent<CameraController>();
        }

        public void GameOver([CanBeNull] GameObject helicopter = null)
        {
            Debug.Log("GAME OVER");

            if (helicopter != null)
            {
                foreach (var player  in Players)
                {
                    if (player == helicopter)
                    {
                        //Particles
                    }
                }
            }
        }

        public void NextCameraStop()
        {
            _cameraController.NextStop();
        }
    }
}