using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Helicoopter
{
    [RequireComponent(typeof(LineRenderer))]
    public class Rope : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private List<RopeSegment> _ropeSegments = new List<RopeSegment>();
        
        [Header("Rope Parameters")]
        [SerializeField] private float _ropeSegmentLength = 0.25f;
        [SerializeField] private int _segmentCount = 35;
        [SerializeField] private float _lineWidth = 0.1f;
        [Tooltip("The times per fixed frame-rate frame it applies the constraint. More times equals to more precision.")]
        [SerializeField] private int _constraintIterations = 50;

        [Header("Positions")]
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        public float RopeDistance { get; private set; }
        public Vector3 EndPosition { get; private set; }

        private void Awake()
        {
            RopeDistance = _ropeSegmentLength * _segmentCount;
        }

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            Vector3 ropeStartPoint = _startPoint.position;

            for (int i = 0; i < _segmentCount; i++)
            {
                _ropeSegments.Add(new RopeSegment(ropeStartPoint));
                ropeStartPoint.y -= _ropeSegmentLength;
            }
        }

        private void Update()
        {
            DrawRope();
        }

        private void FixedUpdate()
        {
            Simulate();
        }

        private void Simulate()
        {
            //SIMULATION
            Vector2 forceGravity = new Vector2(0f, -1.5f);
            
            for (int i = 1; i < _segmentCount; i++)
            {
                RopeSegment firstSegment = _ropeSegments[i];
                Vector2 velocity = firstSegment.PosNow - firstSegment.PosOld;
                firstSegment.PosOld = firstSegment.PosNow;
                firstSegment.PosNow += velocity;
                firstSegment.PosNow += forceGravity * Time.deltaTime;
                _ropeSegments[i] = firstSegment;
            }
            
            //CONTRAINTS
            for (int i = 0; i < _constraintIterations; i++)
            {
                ApplyConstraints();
            }

            EndPosition = _ropeSegments[_segmentCount - 1].PosNow;
        }

        private void ApplyConstraints()
        {
            //Constraint(First segment always follow the start position)
            RopeSegment firstSegment = _ropeSegments[0];
            firstSegment.PosNow = _startPoint.position;
            _ropeSegments[0] = firstSegment;

            //Constraint(Last segment always follow the end position)
            if (_endPoint != null)
            {
                RopeSegment endSegment = _ropeSegments[_segmentCount - 1];
                endSegment.PosNow = _endPoint.position;
                _ropeSegments[_segmentCount - 1] = endSegment;
            }

            //Constraint(Keep fixed distance apart for each points)
            for (int i = 0; i < _segmentCount - 1; i++)
            {
                RopeSegment firstSeg = _ropeSegments[i];
                RopeSegment secondSeg = _ropeSegments[i + 1];

                float dist = (firstSeg.PosNow - secondSeg.PosNow).magnitude;
                float error = Mathf.Abs(dist - _ropeSegmentLength);
                Vector2 changeDir = Vector2.zero;

                if (dist > _ropeSegmentLength)
                {
                    changeDir = (firstSeg.PosNow - secondSeg.PosNow).normalized;
                }
                else if(dist < _ropeSegmentLength)
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
        
        private void DrawRope()
        {
            float lineWidth = _lineWidth;
            _lineRenderer.startWidth = lineWidth;
            _lineRenderer.endWidth = lineWidth;

            Vector3[] ropePositions = new Vector3[_segmentCount];

            for (int i = 0; i < _segmentCount; i++)
            {
                ropePositions[i] = _ropeSegments[i].PosNow;
            }

            _lineRenderer.positionCount = ropePositions.Length;
            _lineRenderer.SetPositions(ropePositions);
        }

        public void SetEndPoint(Transform point)
        {
            _endPoint = point;
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
