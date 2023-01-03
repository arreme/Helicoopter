using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helicoopter
{
    public class EnemyDetector : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            print("Detected enemy! I'm Ded");
        }
    }
}
