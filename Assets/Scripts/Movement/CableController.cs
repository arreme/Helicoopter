using System;
using System.Collections;
using UnityEngine;

namespace Helicoopter
{
    public class CableController : MonoBehaviour
    {
        [SerializeField] private LineRenderer _renderer;
        private Rope rope;
        
        private bool _cableDeployed;

        [Header("Cable Deployment")] 
        [SerializeField] private int maxSegments;
        [SerializeField] private float deploymentTime;
        
        private float _currentDeployed;
        private float _currentCableSpeed;
        private int _segmentsToDraw;
        private bool _hasPackage;


        private void Awake()
        {
            rope = new Rope(_renderer,maxSegments,transform);
        }

        private void Update()
        {
            rope.RenderRope();
        }

        private void FixedUpdate()
        {
            _currentDeployed = Mathf.SmoothDamp(_currentDeployed,_cableDeployed ? 1.0f : 0.0f,ref _currentCableSpeed,deploymentTime);

            _segmentsToDraw = Mathf.RoundToInt(_currentDeployed * maxSegments);
            
            rope.UpdateRope();
        }
        
        

        public void SetCable()
        {
            _cableDeployed = !_cableDeployed;
            StartCoroutine(CableCoroutine(_cableDeployed));
        }

        private IEnumerator CableCoroutine(bool cable)
        {
            for (int i = 0; i < maxSegments; i++)
            {
                rope.ResizeRope(cable);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}