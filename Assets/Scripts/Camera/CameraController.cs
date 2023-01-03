using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Helicoopter
{
    public class CameraController : MonoBehaviour
    {
        private Camera _camera;
        private float _mainZ;

        private GameObject[] _players;

        [Header("Map Positions")] 
        [SerializeField] private Transform _startPoint;
        [SerializeField] private List<MapStop> _mapStops;

        private int _mapStopIndex;
        private bool _ended;
        private Vector3 _camVelocity;
        private float _currentTime;
        private Vector3 _startPosition;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            if(GameState.Instance != null) _players = GameState.Instance.Players;
        }

        private void Start()
        {
            var trans = transform;
            _mainZ = trans.position.z;
            var start = _startPoint.position;
            trans.position = new Vector3(start.x, start.y, _mainZ);
            //_currentTime = _mapStops[_mapStopIndex]._timeToArrive;
            _currentTime = 0f;
            _startPosition = start;
        }

        private void Update()
        {
            var gameOver = false;
            if(_players != null) gameOver = CheckPlayersInGameZone();

            if (_ended || gameOver)
                return;
            
            var mapStop = _mapStops[_mapStopIndex]._stopPosition.position;
            var x = mapStop.x;
            var y = mapStop.y;
            var z = _mainZ;

            mapStop = new Vector3(x, y, z);
            var maxTime = _mapStops[_mapStopIndex]._timeToArrive;

            transform.position = Vector3.Lerp(_startPosition, mapStop, _currentTime/maxTime);
            _currentTime += Time.deltaTime;
        }

        private bool CheckPlayersInGameZone()
        {
            foreach (var player in _players)
            {
                Vector3 viewPos = _camera.WorldToViewportPoint(player.transform.position);

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
            _startPosition = transform.position;
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
