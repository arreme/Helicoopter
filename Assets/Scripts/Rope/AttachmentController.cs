using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helicoopter
{
    public class AttachmentController : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Rope _rope;
        [SerializeField] private RopeAttacher _attacher;

        private DistanceJoint2D _joint;

        private void Awake()
        {
            _joint = GetComponent<DistanceJoint2D>();
        }

        private void Start()
        {
            SetJointConfig();
            _joint.enabled = false;
            _attacher.AttachmentController = this;
        }

        private void Update()
        {
            _attacher.transform.position = _rope.EndPosition;
        }

        private void SetJointConfig()
        {
            _joint.distance = _rope.RopeDistance;
            _joint.maxDistanceOnly = true;
            _joint.enableCollision = true;
        }

        public void Attach(Transform attachable)
        {
            _attacher.gameObject.SetActive(false);
            _rope.SetEndPoint(attachable);
            _joint.enabled = true;
        }
    }
}
