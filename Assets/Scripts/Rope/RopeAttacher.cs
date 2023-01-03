using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helicoopter
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class RopeAttacher : MonoBehaviour
    {
        [SerializeField] private CableController _controller;
        private CircleCollider2D _col;

        void Start()
        {
            _col = GetComponent<CircleCollider2D>();
        }

        public void EnableAttacher(bool enable)
        {
            _col.enabled = enable;
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Attachable"))
            {
                _controller.Attach(other.transform);
            }
        }
    }
}
