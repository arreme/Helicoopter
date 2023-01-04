using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Helicoopter
{
    public class GameState : MonoBehaviour
    {
        public static GameState Instance { get; private set; }

        [HideInInspector] public List<GameObject> Players { get; private set; }
        private List<Attachable> _attachables = new List<Attachable>();
        private CameraController _cameraController;

        [SerializeField] private EndMenu endMenu;
        [SerializeField] private GameObject _videoPanel;
        private VideoPlayer _videoPlayer;
        private bool unkillable = false;

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
            Players = GameObject.FindGameObjectsWithTag("Player").ToList();
            _videoPlayer = GetComponent<VideoPlayer>();
            _videoPanel.SetActive(false);
            
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
            if (!unkillable)
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
                endMenu.EndLevel(true);
            }
        }

        private void WinLevel()
        {
            unkillable = true;
            PlayVideo();
            
            endMenu.EndLevel(false);
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
            }

            bool check = CheckAttachedPicked(obj);

            if (check)
            {
                NextCameraStop();
            }
        }

        private bool CheckAttachedPicked(GameObject obj)
        {
            foreach (var att in _attachables)
            {
                if (att._isAttached && _cameraController.CheckIfAttachedPicked(obj))
                {
                    return true;
                }
            }
            return false;
        }

        private void NextCameraStop()
        {
            _cameraController.NextStop();
        }

        public void ChangeDelivered(GameObject obj, bool state)
        {
            foreach (var att in _attachables)
            {
                if (att._object == obj)
                {
                    att._isAttached = state;
                    break;
                }
            }
            
            bool check = CheckDeliveredObjects(obj);

            if (check)
            {
                WinLevel();
            }
        }

        private bool CheckDeliveredObjects(GameObject o)
        {
            int count = 0; 
            foreach (var att in _attachables)
            {
                if (att._isDelivered)
                {
                    count++;
                }
            }

            return count == _attachables.Count;
        }

        public void PlayVideo()
        {
            _videoPanel.SetActive(true);
            _videoPlayer.Play();
            _videoPlayer.loopPointReached += EndReached;
        }

        private void EndReached(VideoPlayer vp)
        {
            _videoPanel.SetActive(false);
        }
    }
    
    internal class Attachable
    {
        internal GameObject _object;
        internal bool _isAttached;
        internal bool _isDelivered;

        internal Attachable(GameObject obj, bool state)
        {
            _object = obj;
            _isAttached = state;
            _isDelivered = false;
        }
    }
}