using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Helicoopter
{
    public class Rope
    {
        private readonly LineRenderer _lineRenderer;
        private readonly List<RopeSegment> _ropeSegments;

        private const float RopeSegmentLength = 0.25f;
        private const float LineWidth = 0.1f;
        private const int ConstraintIterations = 50;
        private readonly int _maxSegmentCount;

        private int _currentRope;

        private Transform _playerTr;
        private Transform _endPoint;
        private RopeSegment _endRope;
        public float RopeDistance { get; private set; }
        public Vector3 EndPosition { get; private set; }

        public Rope(LineRenderer lineRenderer, int maxSegmentCount, Transform transform)
        {
            _lineRenderer = lineRenderer;
            _maxSegmentCount = maxSegmentCount;
            _ropeSegments = new List<RopeSegment>();
            RopeDistance = RopeSegmentLength * maxSegmentCount;
            _playerTr = transform;
        }
        
        public void RenderRope()
        {
            if (_currentRope <= 1) return;
            DrawRope(_currentRope);
        }
        
        public void UpdateRope()
        {
            Simulate();
            
            // for (int i = 0; i < ConstraintIterations; i++)
            // {
            //     ApplyConstraints();
            // }
            //
            // EndPosition = _ropeSegments[_maxSegmentCount - 1].PosNow;
        }

        

        private void Simulate()
        {
            Vector2 forceGravity = new Vector2(0f, -1.5f);
            
            for (int i = 1; i < _maxSegmentCount; i++)
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
            RopeSegment firstSegment = _ropeSegments[0];
            firstSegment.PosNow = _playerTr.position;
            _ropeSegments[0] = firstSegment;

            //Constraint(Last segment always follow the end position)
            if (_endPoint != null)
            {
                RopeSegment endSegment = _ropeSegments[_maxSegmentCount - 1];
                endSegment.PosNow = _endPoint.position;
                _ropeSegments[_maxSegmentCount - 1] = endSegment;
            }

            //Constraint(Keep fixed distance apart for each points)
            for (int i = 0; i < _maxSegmentCount - 1; i++)
            {
                RopeSegment firstSeg = _ropeSegments[i];
                RopeSegment secondSeg = _ropeSegments[i + 1];

                float dist = (firstSeg.PosNow - secondSeg.PosNow).magnitude;
                float error = Mathf.Abs(dist - RopeSegmentLength);
                Vector2 changeDir = Vector2.zero;

                if (dist > RopeSegmentLength)
                {
                    changeDir = (firstSeg.PosNow - secondSeg.PosNow).normalized;
                }
                else if(dist < RopeSegmentLength)
                {
                    changeDir = (secondSeg.PosNow - firstSeg.PosNow).normalized;
                }

                Vector2 changeAmount = changeDir * error;
                if (i != 0)
                {
                    firstSeg.PosNow -= changeAmount * 0.5f;
                    _ropeSegments[i] = firstSeg;
                    secondSeg.PosNow += changeAmount * 0.5f;
                    _ropeSegments[i + 1] = secondSeg;
                }
                else
                {
                    secondSeg.PosNow += changeAmount;
                    _ropeSegments[i + 1] = secondSeg;
                }
            }
        }
        
        private void DrawRope(int segmentsToDraw)
        {
            float lineWidth = LineWidth;
            _lineRenderer.startWidth = lineWidth;
            _lineRenderer.endWidth = lineWidth;

            Vector3[] ropePositions = new Vector3[segmentsToDraw];

            for (int i = 0; i < segmentsToDraw; i++)
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
            //
            //
            if (moreRope)
            {
                if (_currentRope >= _maxSegmentCount) return;
                if (_currentRope == 0)
                {
                    Vector3 ropeStartPoint = _playerTr.position;
                    _ropeSegments.Add(new RopeSegment(ropeStartPoint));
                    ropeStartPoint.y -= RopeSegmentLength;
                    _endRope = new RopeSegment(ropeStartPoint);
                    _ropeSegments.Add(_endRope);
                    _currentRope++;
                }
                else
                {
                    var changePos = _playerTr.position;
                    changePos.y -= RopeSegmentLength;
                    for (int i = 0; i < _currentRope; i++)
                    {
                        RopeSegment rope = _ropeSegments[i];
                        var temp = rope.PosNow;
                        rope.PosNow = changePos;
                        _ropeSegments[i] = rope;
                        changePos = temp;
                    }
                    _currentRope++;
                    _ropeSegments.Insert(0,new RopeSegment(_playerTr.position));
                }
                
                Debug.Log(_currentRope);

                
            }
            else
            {
                if (_currentRope <= 0) return;
                if (_currentRope == 2)
                {
                    
                }
                else
                {
                    
                }

                _currentRope--;
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
