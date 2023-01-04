using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Helicoopter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class HelicopterController : MonoBehaviour
    {
        private float _inputMovement;

        private bool _engineOn;
        private bool _diveDown;

        private Rigidbody2D _rb;

        [SerializeField] private float sGravity;
        
        [Header("Up Settings")]
        [SerializeField] private float sMaxAcc;
        [SerializeField] private float sMaxAccTime;
        [SerializeField] private AnimationCurve sUpSpeedCurve;
        [SerializeField] private float maxSpeed;

        private float _currentAcc;
        private float _accSpeed;

        [Header("Turn Settings")]
        [SerializeField] private float sInputRate;
        [SerializeField] private float sMaxDegrees;
        private float _turnSpeed;
        private float _direction;

        private Animator _animation;
        [SerializeField] private AudioSource source;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            //GameState.Instance.Players
        }

        private void FixedUpdate()
        {
            //Up and Down
            _currentAcc = Mathf.SmoothDamp(_currentAcc,  (_engineOn ? 1 : 0),ref _accSpeed ,sMaxAccTime);
            float acc = sUpSpeedCurve.Evaluate(_currentAcc) * sMaxAcc;
            _rb.AddForce(_rb.mass * acc * transform.up,ForceMode2D.Force);
            _rb.AddForce(sGravity * (_diveDown ? 1.5f : 1) * (_engineOn ? 1 : 0.5f) * Vector2.down, ForceMode2D.Force);

            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, maxSpeed);

            _direction = Mathf.SmoothDamp(_direction,_inputMovement,ref _turnSpeed,sInputRate);
            _rb.MoveRotation( _direction * sMaxDegrees * -1);
        }

        public void SetEngine(float engine)
        {
            if (Mathf.Approximately(engine, -1))
            {
                _engineOn = false;
                _diveDown = true;
            }
            else
            {
                _diveDown = false;
                if (Mathf.Approximately(engine, 1))
                {
                    _engineOn = true;
                    source.mute = false;
                }
                else if (Mathf.Approximately(engine,0))
                {
                    _engineOn = false;
                    source.mute = true;
                }
            }
            

            _animation.enabled = _engineOn;
            
        }

        public void SetHelixAnim(Animator animation)
        {
            _animation = animation;
        }

        public void SetMovement(float movement)
        {
            _inputMovement = movement;
        }
    }
}
