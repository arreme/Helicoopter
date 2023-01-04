using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helicoopter
{
    public class DropZone : MonoBehaviour
    {
        [SerializeField] private GameObject _deliverable;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Attachable"))
            {
                if (col.gameObject == _deliverable)
                {
                    GameState.Instance.ChangeDelivered(col.gameObject, true);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.CompareTag("Attachable"))
            {
                if (col.gameObject == _deliverable)
                {
                    GameState.Instance.ChangeDelivered(col.gameObject, false);
                }
            }
        }
    }
}
