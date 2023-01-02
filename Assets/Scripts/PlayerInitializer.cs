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
        private void Awake()
        {
            _scheme = new PlayerInputScheme();
           _controller = GetComponent<HelicopterController>();
        }

        public void InitializePlayer(PlayerConfiguration config)
        {
            //SetPlayerColor
            config.Input.onActionTriggered += Input_OnActionTriggered;
        }

        private void Input_OnActionTriggered(InputAction.CallbackContext ctx)
        {
            if (ctx.action == _scheme.Player.Engine)
            {
                TurnEngine(ctx.ReadValue<float>());
            } else if (ctx.action == _scheme.Player.Movement)
            {
                LeftRight(ctx.ReadValue<float>());
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
