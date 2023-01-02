using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Helicoopter
{
    public class SpawnPlayerMenu : MonoBehaviour
    {
        [SerializeField] private PlayerInput input;
        public GameObject playerSetupMenuPrefab;
        
        private void Awake()
        {
            var rootMenu = GameObject.FindWithTag("RootMenu");
            if (!rootMenu) return;
            var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
            input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<PlayerInputSetupMenu>().SetPlayerIndex(input.playerIndex);
        }
    }
}
