using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helicoopter
{
    [RequireComponent(typeof(HelicopterController))]
    public class PlayerHandler : MonoBehaviour
    {
        private PlayerInputScheme _scheme;
        private HelicopterController _helicopterController;

        private void Awake()
        {
            _helicopterController = GetComponent<HelicopterController>();
            _scheme = new PlayerInputScheme();
            _scheme.Player.Movement.performed += ctx => LeftRight(ctx.ReadValue<float>());
            _scheme.Player.Engine.performed += ctx => TurnEngine(ctx.ReadValue<float>());
        }

        private void TurnEngine(float engine)
        {
            _helicopterController.SetEngine(Mathf.Approximately(engine,1));
        }

        private void LeftRight(float input)
        {
            _helicopterController.SetMovement(input);
        }

        private void OnEnable()
        {
            _scheme.Enable();
        }

        private void OnDisable()
        {
            _scheme.Disable();
        }
    }
}
