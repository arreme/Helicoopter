using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Helicoopter
{
    public class PlayerInputSetupMenu : MonoBehaviour
    {
        private int _playerIndex;

        [SerializeField] private TextMeshProUGUI tittleText;
        [SerializeField] private GameObject readyPanel;
        [SerializeField] private Button readyButton;
        [SerializeField] private InputSystemUIInputModule eventSystem;
        [SerializeField] private Image helicopterImage;
        [SerializeField] private Image helix1;
        [SerializeField] private Image helix2;
        [SerializeField] private SpriteRenderer leftButton;
        [SerializeField] private SpriteRenderer rightButton;
        
        private readonly float _ignoreInputTime = 0.2f;
        private bool _inputEnabled;
        private static bool _loaded;
        private static Sprite[] _helicotperAssets;
        private int _currentAsset;
        private bool _colorSelected;
        private bool _isHelix2;
        private void Awake()
        {
            if (!_loaded)
            {
                _helicotperAssets = Resources.LoadAll<Sprite>("Helicopters").ToArray();
                _loaded = true;
            }
            
            helicopterImage.sprite = _helicotperAssets[_currentAsset];
            helix1.gameObject.SetActive(!_isHelix2); 
            helix2.gameObject.SetActive(_isHelix2); 
        }

        private void HelicopterMenu(Vector2 ctx)
        {
            if (!_inputEnabled || _colorSelected) return;
            if (_colorSelected) return;
            if (ctx.x > 0)
            {
                _currentAsset++;
                if (_currentAsset >= _helicotperAssets.Length)
                {
                    _currentAsset = 0;
                }
            }
            else
            {
                _currentAsset--;
                if (_currentAsset < 0)
                {
                    _currentAsset = _helicotperAssets.Length - 1;
                }
            }
            
            UpdateAsset();
        }

        private void UpdateAsset()
        {
            helicopterImage.sprite = _helicotperAssets[_currentAsset];
            if (_currentAsset > 8 || _currentAsset == 0)
            {
                _isHelix2 = false;
                helix1.gameObject.SetActive(!_isHelix2); 
                helix2.gameObject.SetActive(_isHelix2); 
            }
            else
            {
                _isHelix2 = true;
                helix1.gameObject.SetActive(!_isHelix2); 
                helix2.gameObject.SetActive(_isHelix2); 
            }
        }

        public void SetPlayerIndex(PlayerInput pi)
        {
            _playerIndex = pi.playerIndex;
            tittleText.SetText("Player" + (_playerIndex + 1));
            pi.SwitchCurrentActionMap("UI");
            for (int i = 0; i < pi.currentActionMap.actions.Count; i++)
            {
                if (pi.currentActionMap.actions[i].name == "Navigate")
                {
                    pi.currentActionMap.actions[i].performed += ctx => HelicopterMenu(ctx.ReadValue<Vector2>());
                } else if (pi.currentActionMap.actions[i].name == "Submit")
                {
                    pi.currentActionMap.actions[i].performed += _ => SetColor();
                }
            }
            StartCoroutine(EnableInput());
        }

        private IEnumerator EnableInput()
        {
            yield return new WaitForSecondsRealtime(_ignoreInputTime);
            print("You can do this!");
            _inputEnabled = true;
        }

        private void SetColor()
        {
            if (!_inputEnabled || _colorSelected) return;
            S_PlayerInputManager.Instance.SetPlayerColor(_playerIndex,_helicotperAssets[_currentAsset],_isHelix2);
            print("Hey");
            readyPanel.SetActive(true);
            readyButton.Select();
            _inputEnabled = false;
            StartCoroutine(EnableInput());
            _colorSelected = true;
        }
        

        public void ReadyPlayer()
        {
            print("Bug!");
            if (!_inputEnabled) return;
            S_PlayerInputManager.Instance.ReadyPlayer(_playerIndex);
            readyButton.gameObject.SetActive(false);
        }
    }
    
}
