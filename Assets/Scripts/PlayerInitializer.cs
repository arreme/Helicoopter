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
        private HelicopterController _controller;
        private PlayerInputScheme _scheme;
        private PlayerConfiguration _config;
        private void Awake()
        { 
            _scheme = new PlayerInputScheme(); 
            _controller = GetComponent<HelicopterController>();
        }

        public void InitializePlayer(PlayerConfiguration config)
        {
            //SetPlayerColor
            _config = config;
            for (int i = 0; i < _config.Input.currentActionMap.actions.Count; i++)
            {
                if (_config.Input.currentActionMap.actions[i].name == _scheme.Player.Engine.name)
                {
                    config.Input.currentActionMap.actions[i].performed += ctx => TurnEngine(ctx.ReadValue<float>());
                } else if (_config.Input.currentActionMap.actions[i].name == _scheme.Player.Movement.name)
                {
                    config.Input.currentActionMap.actions[i].performed += ctx => LeftRight(ctx.ReadValue<float>());
                }
            }
        }

        private void TurnEngine(float engine)
        {
            _controller.SetEngine(Mathf.Approximately(engine,1));
        }
        
        private void LeftRight(float input)
        {
            _controller.SetMovement(input);
        }
    }
}
