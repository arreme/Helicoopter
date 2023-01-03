using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
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
        [SerializeField] private InputSystemUIInputModule eventSystem;

        private readonly float _ignoreInputTime = 1.5f;
        private bool _inputEnabled;

        private static bool _loaded;
        private static Sprite[] _helicotperAssets;

        private void Awake()
        {
            if (!_loaded)
            {
                _helicotperAssets = Resources.LoadAll<Sprite>("Helicotpers/");
                _loaded = true;
            }
            eventSystem.move.action.performed += ctx => HelicopterMenu(ctx.ReadValue<Vector2>());
        }

        private void HelicopterMenu(Vector2 ctx)
        {
            print(ctx);
        }

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
            bool colorAssigned = false;
            foreach (Sprite helicopter in _helicotperAssets)
            {
                if (helicopter.name.Equals(color))
                {
                    S_PlayerInputManager.Instance.SetPlayerColor(_playerIndex,helicopter);
                    colorAssigned = true;
                }
            }
            if (!colorAssigned) print("Error");
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
    
}
