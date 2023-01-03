using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helicoopter
{
    public class EnemyPatrol : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform[] patrolPoints;

        [SerializeField] private float speed;
        
        private Transform _patrolPoint;
        private int _currentPatrol;
        private Vector3 _currSped;
        private void Awake()
        {
            _patrolPoint = patrolPoints[_currentPatrol];
        }

        private void FixedUpdate()
        {
            target.position = Vector3.SmoothDamp(target.position, _patrolPoint.position, ref _currSped, speed);
            if ((target.position - _patrolPoint.position).magnitude <= 0.1f)
            {
                _currentPatrol++;
                if (_currentPatrol >= patrolPoints.Length) _currentPatrol = 0;
                _patrolPoint = patrolPoints[_currentPatrol];
            }
        }
    }
}
