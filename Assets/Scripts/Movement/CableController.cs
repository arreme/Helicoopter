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
        private bool _inCooldown;

        [Header("Cable Deployment")] 
        [SerializeField] private int maxSegments;
        [SerializeField] private float deploymentTime;
        
        private float _currentDeployed;
        private float _currentCableSpeed;
        private bool _hasPackage;

        [SerializeField] private AudioSource source;

        private void Awake()
        {
            _rope = new Rope(lineRenderer,maxSegments,transform);
            join2D.distance = _rope.MaxDistance * 1.2f;
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
            if (_inCooldown) return;
            _inCooldown = true;
            _cableDeployed = !_cableDeployed;
            if (_cableDeployed) {S_AudioManager.AudioManager.SetAudioClip(source, AudioClips.GetRope);}
            else {S_AudioManager.AudioManager.SetAudioClip(source, AudioClips.ShootRope);}
            StartCoroutine(CableCoroutine(_cableDeployed));
            if (!_cableDeployed)
            {
                DeAttach();
            }
        }

        public void Attach(Transform attacheable)
        {
            if (join2D.connectedBody == null && attacheable.TryGetComponent(out Rigidbody2D rb))
            {
                _rope.SetEndPoint(attacheable);
                ropeAttacher.EnableAttacher(false);
                join2D.connectedBody = rb;
                join2D.enabled = true;
                if (GameState.Instance != null) GameState.Instance.ChangeAttachable(attacheable.gameObject,true);
                if (attacheable.TryGetComponent(out AudioSource source))
                {
                    source.Play();
                }
            }
            
        }

        private void DeAttach()
        {
            
            if (join2D.connectedBody != null)
            {
                _rope.SetEndPoint(null);
                join2D.enabled = false;
                if (join2D.connectedBody.gameObject.TryGetComponent(out AudioSource source))
                {
                    source.Play();
                }
                if (GameState.Instance != null)
                {
                    GameState.Instance.ChangeAttachable(join2D.connectedBody.gameObject,false);
                }
                join2D.connectedBody = null;
            }
            
        }

        private IEnumerator CableCoroutine(bool cable)
        {
            
            for (int i = 0; i < maxSegments + 2; i++)
            {
                _rope.ResizeRope(cable);
                yield return new WaitForSeconds(deploymentTime/maxSegments);
            }


            _inCooldown = false;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}