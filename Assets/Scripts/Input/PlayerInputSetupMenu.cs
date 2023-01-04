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
        private PlayerInput _playerIndex;

        [SerializeField] private TextMeshProUGUI tittleText;
        [SerializeField] private GameObject readyPanel;
        [SerializeField] private Button readyButton;
        [SerializeField] private InputSystemUIInputModule eventSystem;
        [SerializeField] private Image helicopterImage;
        [SerializeField] private Image helix1;
        [SerializeField] private Image helix2;
        [SerializeField] private GameObject selectPanel;

        private AudioSource _source;
        private Image _buttonLeft;
        private Image _buttonRight;
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
            _buttonRight = selectPanel.transform.GetChild(0).GetComponent<Image>();
            _buttonLeft = selectPanel.transform.GetChild(1).GetComponent<Image>();
            _source = GetComponent<AudioSource>();
        }

        private void HelicopterMenu(Vector2 ctx)
        {
            if (!_inputEnabled || _colorSelected) return;
            _inputEnabled = false;
            S_AudioManager.AudioManager.SetAudioClip(_source,AudioClips.UiInteract);
            if (ctx.y != 0) return;
            if (ctx.x > 0)
            {
                StartCoroutine(ChangeButtonColor(false));
                _currentAsset++;
                if (_currentAsset >= _helicotperAssets.Length)
                {
                    _currentAsset = 0;
                }
            }
            else
            {
                StartCoroutine(ChangeButtonColor(true));
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
            StartCoroutine(EnableInput());
        }
        

        public void SetPlayerIndex(PlayerInput pi)
        {
            S_AudioManager.AudioManager.SetAudioClip(_source,AudioClips.UiJoin);
            _playerIndex = pi;
            tittleText.text = "Player "+pi.playerIndex;
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

        private IEnumerator ChangeButtonColor(bool isLeft)
        {
            if (isLeft)
            {
                _buttonLeft.color = Color.green;
                yield return new WaitForSecondsRealtime(0.2f);
                _buttonLeft.color = Color.white;
            }
            else
            {
                _buttonRight.color = Color.green;
                yield return new WaitForSecondsRealtime(0.2f);
                _buttonRight.color = Color.white;
            }
        }

        private IEnumerator EnableInput()
        {
            yield return new WaitForSecondsRealtime(_ignoreInputTime);
            _inputEnabled = true;
        }

        private void SetColor()
        {
            if (!_inputEnabled) return;
            if (_colorSelected)
            {
                ReadyPlayer();
            }

            _inputEnabled = false;
            S_PlayerInputManager.Instance.SetPlayerColor(_playerIndex.playerIndex,_helicotperAssets[_currentAsset],_isHelix2);
            readyPanel.SetActive(true);
            _colorSelected = true;
            selectPanel.SetActive(false);
            StartCoroutine(EnableInput());
        }
        
        public void ReadyPlayer()
        {
            S_PlayerInputManager.Instance.ReadyPlayer(_playerIndex.playerIndex);
            readyButton.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_playerIndex == null) return;
            for (int i = 0; i < _playerIndex.currentActionMap.actions.Count; i++)
            {
                if (_playerIndex.currentActionMap.actions[i].name == "Navigate")
                {
                    _playerIndex.currentActionMap.actions[i].performed += ctx => HelicopterMenu(ctx.ReadValue<Vector2>());
                } else if (_playerIndex.currentActionMap.actions[i].name == "Submit")
                {
                    _playerIndex.currentActionMap.actions[i].performed += _ => SetColor();
                }
            }
        }
    }
    
}
