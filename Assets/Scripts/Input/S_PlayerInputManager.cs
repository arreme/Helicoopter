using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Helicoopter
{
    public class S_PlayerInputManager : MonoBehaviour
    {
        private List<PlayerConfiguration> _configurations;

        [SerializeField] private int maxPlayers = 2;
        [SerializeField] private string sceneName;

        public static S_PlayerInputManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _configurations = new List<PlayerConfiguration>();
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public PlayerConfiguration[] GetPlayerConfigs()
        {
            return _configurations.ToArray();
        }

        public void SetPlayerColor(int index, Sprite color, bool helix2)
        {
            _configurations[index].Color = color;
            _configurations[index]._helixNumber = helix2 ? 1 : 0;
        }

        private bool _sceneLoaded;
        
        public void ReadyPlayer(int index)
        {
            if (_sceneLoaded) return;
            _configurations[index].IsReady = true;
            if (_configurations.Count == maxPlayers && _configurations.All(x => x.IsReady))
            {
                _sceneLoaded = true;
                SceneManager.LoadScene("StartMenu");
            }
        }

        public void PlayerJoinHandler(PlayerInput pi)
        {

            pi.transform.SetParent(transform);
            if (_configurations.All(p => p.PlayerIndex != pi.playerIndex))
            {
                _configurations.Add(new PlayerConfiguration(pi));
            }
            
        }
    }

    public class PlayerConfiguration
    {
        public PlayerInput Input { get; private set; }
        public int PlayerIndex { get; private set; }
        public bool IsReady { get; set; }
        public Sprite Color { get; set; }

        public int _helixNumber;

        public PlayerConfiguration(PlayerInput input)
        {
            Input = input;
            PlayerIndex = input.playerIndex;
            IsReady = false;
        }
    }
}
