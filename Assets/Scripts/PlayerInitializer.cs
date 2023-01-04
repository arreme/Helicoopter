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
                    config.Input.currentActionMap.actions[i].performed += ctx => TurnEngine(ctx.ReadValue<float>());
                } else if (_config.Input.currentActionMap.actions[i].name == _scheme.Player.Movement.name)
                {
                    config.Input.currentActionMap.actions[i].performed += ctx => LeftRight(ctx.ReadValue<float>());
                } else if (_config.Input.currentActionMap.actions[i].name == _scheme.Player.Cable.name)
                {
                    config.Input.currentActionMap.actions[i].performed += ctx => Cable(ctx.ReadValue<float>());
                }
            }
        }

        private void TurnEngine(float engine)
        {
            _controller.SetEngine(engine);
        }
        
        private void LeftRight(float input)
        {
            _controller.SetMovement(input);
        }

        private void Cable(float input)
        {
            if (Mathf.Approximately(input,1))
            {
                _cableController.SetCable();
            }
            
        }
    }
}
