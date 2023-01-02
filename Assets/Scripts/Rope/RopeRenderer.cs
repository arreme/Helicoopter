using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Helicoopter
{
    public class RopeRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private List<Transform> _points;
        private void Update()
        {
            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPositions(_points.Select(p=> p.position).ToArray());
        }
    }
}
