using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Helicoopter
{
    public class CameraController : MonoBehaviour
    {
        private Camera _mainCamera;
        private Transform _mainTransform;
        private float _mainZ;

        private GameObject[] _players;

        [Header("Map Positions")] 
        [SerializeField] private Transform _startPoint;
        [SerializeField] private List<MapStop> _mapStops;

        private int _mapStopIndex;
        private bool _ended;
        private Vector3 _camVelocity;
        private float _currentTime;

        private void Awake()
        {
            if (Camera.main != null) _mainCamera = Camera.main;
            if (Camera.main != null) _mainTransform = Camera.main.transform;

            _players = GameState.Instance.Players;
        }

        private void Start()
        {
            _mainZ = _mainTransform.transform.position.z;
            var start = _startPoint.position;
            _mainTransform.position = new Vector3(start.x, start.y, _mainZ);
            _currentTime = _mapStops[_mapStopIndex]._timeToArrive;
        }

        private void Update()
        {
            bool gameOver = CheckPlayersInGameZone();

            if (_ended || gameOver)
                return;
            
            var mapStop = _mapStops[_mapStopIndex]._stopPosition.position;
            var x = mapStop.x;
            var y = mapStop.y;
            var z = _mainZ;

            mapStop = new Vector3(x, y, z);

            _mainTransform.position = Vector3.SmoothDamp(_mainTransform.position, mapStop, ref _camVelocity, _currentTime);

            if (_currentTime > 0f)
                _currentTime -= Time.deltaTime;
        }

        private bool CheckPlayersInGameZone()
        {
            foreach (var player in _players)
            {
                Vector3 viewPos = _mainCamera.WorldToViewportPoint(player.transform.position);

                if (viewPos.x is >= 0 and <= 1 && viewPos.y is >= 0 and < 1)
                {
                    GameState.Instance.GameOver(player);
                    return true;
                }
            }

            return false;
        }

        public void NextStop()
        {
            _mapStopIndex++;
            if (_mapStopIndex >= _mapStops.Count)
            {
                _ended = true;
            }
            else
            {
                _currentTime = _mapStops[_mapStopIndex]._timeToArrive;
            }
        }

        [Serializable]
        private struct MapStop
        {
            [SerializeField] internal Transform _stopPosition;
            [SerializeField] internal float _timeToArrive;
        }
    }
}
