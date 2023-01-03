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

        [SerializeField] private float timeToTarget;
        
        private Transform _patrolPoint;
        private int _currentPatrol=1;
        private float _currentT;
        private Vector3 _lastPosition;
        private void Awake()
        {
            _patrolPoint = patrolPoints[_currentPatrol];
            _lastPosition = target.position;
        }

        private void FixedUpdate()
        {
            _currentT += Time.fixedDeltaTime;
            target.position = Vector3.Lerp(_lastPosition, _patrolPoint.position, _currentT/timeToTarget );
            if (_currentT > timeToTarget)
            {
                _currentPatrol++;
                if (_currentPatrol >= patrolPoints.Length) _currentPatrol = 0;
                _patrolPoint = patrolPoints[_currentPatrol];
                _currentT = 0;
                _lastPosition = target.position;
            }
            
        }
    }
}
