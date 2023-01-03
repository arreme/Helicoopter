using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Helicoopter
{
    public class GameState : MonoBehaviour
    {
        public static GameState Instance { get; private set; }

        [HideInInspector] public GameObject[] Players { get; private set; }
        private List<Attachable> _attachables = new List<Attachable>();
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
            Players = GameObject.FindGameObjectsWithTag("Player");
            
            FillAttachables();
        }

        private void FillAttachables()
        {
            GameObject[] attachObj = GameObject.FindGameObjectsWithTag("Attachable");
            foreach (var attach in attachObj)
            {
                _attachables.Add(new Attachable(attach, false));
            }
        }

        private void Start()
        {
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

        public void ChangeAttachable(GameObject obj ,bool state)
        {
            foreach (var att in _attachables)
            {
                if (att._object == obj)
                {
                    att._isAttached = state;
                    break;
                }
                Debug.Log(att._object.name + " " + att._isAttached);
            }

            bool check = CheckAttachedPicked();
            
            if(check)
                NextCameraStop();
        }

        private bool CheckAttachedPicked()
        {
            int count = 0; 
            foreach (var att in _attachables)
            {
                if (att._isAttached)
                {
                    count++;
                }
            }

            return count == _attachables.Count;
        }

        private void NextCameraStop()
        {
            _cameraController.NextStop();
        }
    }
    
    internal class Attachable
    {
        internal GameObject _object;
        internal bool _isAttached;

        internal Attachable(GameObject obj, bool state)
        {
            _object = obj;
            _isAttached = state;
        }
    }
}