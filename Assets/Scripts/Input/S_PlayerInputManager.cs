using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Helicoopter
{
    public class S_PlayerInputManager : MonoBehaviour
    {
        private List<PlayerConfiguration> _configurations;

        [SerializeField] private int maxPlayers = 2;

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

        public void SetPlayerColor(int index, PlayerColor color)
        {
            _configurations[index].SetColor(color);
        }

        public void ReadyPlayer(int index)
        {
            _configurations[index].SetReady(true);
            if (_configurations.Count == maxPlayers && _configurations.All(x => x.GetReady()))
            {
                //START GAME
            }
        }

        public void PlayerJoinHandler(PlayerInput pi)
        {

            pi.transform.SetParent(transform);
            if (_configurations.All(p => p.GetPlayerIndex() != pi.playerIndex))
            {
                _configurations.Add(new PlayerConfiguration(pi));
            }
            
        }
    }

    public struct PlayerConfiguration
    {
        private readonly PlayerInput _input;
        private readonly int _playerIndex;
        private bool _isReady;
        private PlayerColor _color;

        public PlayerConfiguration(PlayerInput input)
        {
            _input = input;
            _playerIndex = input.playerIndex;
            _color = PlayerColor.Yellow;
            _isReady = false;
        }

        public void SetColor(PlayerColor color)
        {
            _color = color;
        }

        public PlayerColor GetColor()
        {
            return _color;
        }

        public void SetReady(bool ready)
        {
            _isReady = ready;
        }

        public bool GetReady()
        {
            return _isReady;
        }

        public int GetPlayerIndex()
        {
            return _playerIndex;
        }

        public PlayerInput GetPlayerInput()
        {
            return _input;
        }
    }
}
