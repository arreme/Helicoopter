using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Helicoopter
{
    public class PlayerInputSetupMenu : MonoBehaviour
    {
        private int _playerIndex;

        [SerializeField] private TextMeshProUGUI tittleText;
        [SerializeField] private GameObject readyPanel;
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private Button readyButton;
        
        private readonly float _ignoreInputTime = 1.5f;
        private bool _inputEnabled;

        public void SetPlayerIndex(int pi)
        {
            _playerIndex = pi;
            tittleText.SetText("Player" + (pi + 1));
            StartCoroutine(EnableInput());
        }

        private IEnumerator EnableInput()
        {
            yield return new WaitForSecondsRealtime(_ignoreInputTime);
            _inputEnabled = true;
        }

        public void SetColor(string color)
        {
            if (!_inputEnabled) return;
            Enum.TryParse(color, true, out PlayerColor result);
            S_PlayerInputManager.Instance.SetPlayerColor(_playerIndex,result);
            readyPanel.SetActive(true);
            readyButton.Select();
            menuPanel.SetActive(false);
        }

        public void ReadyPlayer()
        {
            if (!_inputEnabled) return;
            S_PlayerInputManager.Instance.ReadyPlayer(_playerIndex);
            readyButton.gameObject.SetActive(false);
        }
        
        
    }

    public enum PlayerColor
    {
        Normal,
        Kirby,
        Fbi,
        Sky,
        Galaxy,
        Commando,
        Mystery,
        Anime
    }
}
