using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace Helicoopter
{
    public class Rope
    {
        private readonly LineRenderer _lineRenderer;
        private readonly RopeSegment[] _ropeSegments;

        private const float RopeSegmentLength = 0.25f;
        private const float LineWidth = 0.1f;
        private const int ConstraintIterations = 50;
        private readonly int _maxSegmentCount;

        private int _pointCount;

        private Transform _playerTr;
        private Transform _endPoint;
        private RopeSegment _endRope;
        public float RopeDistance { get; private set; }
        public Vector3 EndPosition { get; private set; }

        public Rope(LineRenderer lineRenderer, int maxSegmentCount, Transform transform)
        {
            _lineRenderer = lineRenderer;
            _lineRenderer.enabled = false;
            _maxSegmentCount = maxSegmentCount;
            RopeDistance = RopeSegmentLength * maxSegmentCount;
            _playerTr = transform;
            _ropeSegments = new RopeSegment[_maxSegmentCount];
            _ropeSegments[0] = new RopeSegment(_playerTr.position);
            _pointCount = 1;
        }
        
        public void RenderRope()
        {
            if (_pointCount <= 1) return;
            DrawRope();
        }
        
        public void UpdateRope()
        {
            Simulate();
            for (int i = 0; i < ConstraintIterations; i++)
            {
                ApplyConstraints();
            }
            
            //EndPosition = _ropeSegments[_maxSegmentCount - 1].PosNow;
        }

        

        private void Simulate()
        {
            Vector2 forceGravity = new Vector2(0f, -1.5f);
            
            for (int i = _pointCount - 1; i >= 0; i--)
            {
                RopeSegment firstSegment = _ropeSegments[i];
                Vector2 velocity = firstSegment.PosNow - firstSegment.PosOld;
                firstSegment.PosOld = firstSegment.PosNow;
                firstSegment.PosNow += velocity;
                firstSegment.PosNow += forceGravity * Time.deltaTime;
                _ropeSegments[i] = firstSegment;
            }
        }

        private void ApplyConstraints()
        {
            //Constraint(First segment always follow the start position)
            RopeSegment firstSegment = _ropeSegments[_pointCount - 1];
            firstSegment.PosNow = _playerTr.position;
            _ropeSegments[_pointCount - 1] = firstSegment;

            //Constraint(Last segment always follow the end position)
            if (_endPoint != null)
            {
                RopeSegment endSegment = _ropeSegments[_pointCount - 1];
                endSegment.PosNow = _endPoint.position;
                _ropeSegments[_pointCount - 1] = endSegment;
            }

            //Constraint(Keep fixed distance apart for each points)
            for (int i = _pointCount - 1; i > 0; i--)
            {
                RopeSegment firstSeg = _ropeSegments[i];
                RopeSegment secondSeg = _ropeSegments[i - 1];

                float dist = (firstSeg.PosNow - secondSeg.PosNow).magnitude;
                float error = Mathf.Abs(dist - RopeSegmentLength);
                Vector2 changeDir = Vector2.zero;

                if (dist > RopeSegmentLength)
                {
                    changeDir = (firstSeg.PosNow - secondSeg.PosNow).normalized;
                }

                Vector2 changeAmount = changeDir * error;
                if (i != _pointCount - 1)
                {
                    firstSeg.PosNow -= changeAmount * 0.5f;
                    _ropeSegments[i] = firstSeg;
                    secondSeg.PosNow += changeAmount * 0.5f;
                    _ropeSegments[i - 1] = secondSeg;
                }
                else
                {
                    secondSeg.PosNow += changeAmount;
                    _ropeSegments[i - 1] = secondSeg;
                }
            }
        }
        
        private void DrawRope()
        {
            float lineWidth = LineWidth;
            _lineRenderer.startWidth = lineWidth;
            _lineRenderer.endWidth = lineWidth;

            Vector3[] ropePositions = new Vector3[_pointCount];

            for (int i = 0; i < _pointCount; i++)
            {
                ropePositions[i] = _ropeSegments[i].PosNow;
            }

            _lineRenderer.positionCount = ropePositions.Length;
            _lineRenderer.SetPositions(ropePositions);
        }

        public void SetEndPoint(Transform point)
        {
            //_endPoint = point;
        }

        public void ResizeRope(bool moreRope)
        {
            if (moreRope)
            {
                if (_pointCount >= _maxSegmentCount) return;

                _ropeSegments[_pointCount] = new RopeSegment(_playerTr.position);
                _pointCount++;

                if (_pointCount == 2)
                {
                    _lineRenderer.enabled = true;
                }

            }
            else
            {
                if (_pointCount <= 1) return;
                
                _pointCount--;
                
                if (_pointCount == 2)
                {
                    _lineRenderer.enabled = false;
                }
            }
        }

        public struct RopeSegment
        {
            public Vector2 PosNow;
            public Vector2 PosOld;

            public RopeSegment(Vector2 pos)
            {
                PosNow = pos;
                PosOld = pos;
            }
        }
    }
}
