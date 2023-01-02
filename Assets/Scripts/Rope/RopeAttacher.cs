using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helicoopter
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class RopeAttacher : MonoBehaviour
    {
        private CircleCollider2D _col;
        public AttachmentController AttachmentController;

        void Start()
        {
            _col = GetComponent<CircleCollider2D>();
            _col.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Attachable"))
            {
                AttachmentController.Attach(other.transform);
            }
        }
    }
}
