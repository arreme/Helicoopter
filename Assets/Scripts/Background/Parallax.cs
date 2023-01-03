using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helicoopter
{
    public class Parallax : MonoBehaviour
    {
        private float length;
        private float startPos;
        private Camera _camera;
        public float ParallaxEffect;

        private void Start()
        {
            _camera = Camera.main;
            startPos = transform.position.x;
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void Update()
        {

            float temp = (_camera.transform.position.x * (1 - ParallaxEffect));
            float distance = (_camera.transform.position.x * ParallaxEffect);

            transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

            if (temp > startPos + length) startPos += length;
            else if (temp < startPos - length) startPos -= length;
        }
    }
}
