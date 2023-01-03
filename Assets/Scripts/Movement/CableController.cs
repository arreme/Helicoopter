using System;
using System.Collections;
using UnityEngine;

namespace Helicoopter
{
    public class CableController : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private DistanceJoint2D join2D;
        [SerializeField] private RopeAttacher ropeAttacher;
        
        private Rope _rope;
        
        private bool _cableDeployed;

        [Header("Cable Deployment")] 
        [SerializeField] private int maxSegments;
        [SerializeField] private float deploymentTime;
        
        private float _currentDeployed;
        private float _currentCableSpeed;
        private bool _hasPackage;


        private void Awake()
        {
            _rope = new Rope(lineRenderer,maxSegments,transform);
            join2D.distance = _rope.MaxDistance;
        }

        private void Update()
        {
            _rope.RenderRope();
        }

        private void FixedUpdate()
        {
            _rope.UpdateRope();
            ropeAttacher.EnableAttacher(_cableDeployed);
            ropeAttacher.SetPosition(_rope.GetEndPos());
        }
        
        public void SetCable()
        {
            _cableDeployed = !_cableDeployed;
            StopCoroutine("CableCoroutine");
            StartCoroutine(CableCoroutine(_cableDeployed));
            if (!_cableDeployed)
            {
                DeAttach();
            }
        }

        public void Attach(Transform attacheable)
        {
            if (attacheable.TryGetComponent(out Rigidbody2D rb))
            {
                _rope.SetEndPoint(attacheable);
                ropeAttacher.EnableAttacher(false);
                join2D.connectedBody = rb;
                join2D.enabled = true;
            }
            
        }

        private void DeAttach()
        {
            _rope.SetEndPoint(null);
            join2D.enabled = false;
            join2D.connectedBody = null;
        }

        private IEnumerator CableCoroutine(bool cable)
        {
            for (int i = 0; i < maxSegments + 2; i++)
            {
                _rope.ResizeRope(cable);
                yield return new WaitForSeconds(deploymentTime/maxSegments);
            }
        }
    }
}