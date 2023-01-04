using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Helicoopter
{
    [RequireComponent(typeof(HelicopterController))]
    public class PlayerInitializer : MonoBehaviour
    {
        [SerializeField] private GameObject[] helixes;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private HelicopterController _controller;
        private PlayerInputScheme _scheme;
        private PlayerConfiguration _config;
        private CableController _cableController;
        
        private void Awake()
        { 
            _scheme = new PlayerInputScheme(); 
            _controller = GetComponent<HelicopterController>();
            _cableController = GetComponent<CableController>();
        }

        public void InitializePlayer(PlayerConfiguration config)
        {
            _config = config;
            helixes[_config._helixNumber].SetActive(true);
            spriteRenderer.sprite = config.Color;
            _config.Input.SwitchCurrentActionMap("Player");
            for (int i = 0; i < _config.Input.currentActionMap.actions.Count; i++)
            {
                if (_config.Input.currentActionMap.actions[i].name == _scheme.Player.Engine.name)
                {
                    config.Input.currentActionMap.actions[i].performed += TurnEngine;
                } else if (_config.Input.currentActionMap.actions[i].name == _scheme.Player.Movement.name)
                {
                    config.Input.currentActionMap.actions[i].performed += LeftRight;
                } else if (_config.Input.currentActionMap.actions[i].name == _scheme.Player.Cable.name)
                {
                    config.Input.currentActionMap.actions[i].performed += Cable;
                }
            }
        }

        public void TurnEngine(InputAction.CallbackContext ctx)
        {
            _controller.SetEngine(ctx.ReadValue<float>());
        }
        
        public void LeftRight(InputAction.CallbackContext ctx)
        {
            _controller.SetMovement(ctx.ReadValue<float>());
        }

        public void Cable(InputAction.CallbackContext ctx)
        {
            if (Mathf.Approximately(ctx.ReadValue<float>(),1))
            {
                _cableController.SetCable();
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _config.Input.currentActionMap.actions.Count; i++)
            {
                if (_config.Input.currentActionMap.actions[i].name == _scheme.Player.Engine.name)
                {
                    _config.Input.currentActionMap.actions[i].performed -= TurnEngine;
                } else if (_config.Input.currentActionMap.actions[i].name == _scheme.Player.Movement.name)
                {
                    _config.Input.currentActionMap.actions[i].performed -= LeftRight;
                } else if (_config.Input.currentActionMap.actions[i].name == _scheme.Player.Cable.name)
                {
                    _config.Input.currentActionMap.actions[i].performed -= Cable;
                }
            }
        }
    }
}
