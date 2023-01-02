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
        private bool _inputEngine;
        private float _inputMovement;

        private Rigidbody2D _rb;

        [SerializeField] private float sGravity;
        
        [Header("Up Settings")]
        [SerializeField] private float sMaxAcc;
        [SerializeField] private float sMaxAccTime;
        [SerializeField] private AnimationCurve sUpSpeedCurve;

        private float _currentAcc = 0;

        [Header("Turn Settings")]
        [SerializeField] private float sInputRate;
        [SerializeField] private float sMaxDegrees;
        private float _turnSpeed;
        private float _direction;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            //Up and Down
            _currentAcc = Mathf.Clamp(_currentAcc + (Time.deltaTime * (_inputEngine ? 1 : -1)),0,sMaxAccTime);
            float acc = sUpSpeedCurve.Evaluate(_currentAcc) * sMaxAcc;
            _rb.AddForce(_rb.mass * acc * transform.up,ForceMode2D.Force);
            _rb.AddForce(sGravity * Vector2.down, ForceMode2D.Force);
            
            //Movement
            _direction = Mathf.SmoothDamp(_direction,_inputMovement,ref _turnSpeed,sInputRate);
            transform.eulerAngles = _direction * sMaxDegrees * Vector3.back;
        }

        public void SetEngine(bool engine)
        {
            _inputEngine = engine;
        }

        public void SetMovement(float movement)
        {
            _inputMovement = movement;
        }
    }
}
