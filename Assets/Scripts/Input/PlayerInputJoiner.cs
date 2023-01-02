using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Helicoopter
{
    public class PlayerInputJoiner : MonoBehaviour
    {
        [SerializeField] private PlayerInputManager manager;
        private List<int> _deviceIds;
        
        private int _id;
        private bool _firstKeyboard;
        private bool _secondKeyboard;
        private bool _inputEnabled;

        private void Awake()
        {
            _deviceIds = new List<int>();
            StartCoroutine(Delay());
        }
        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.5f);
            _inputEnabled = true;
        }

        void Update()
        {
            if (!_inputEnabled) return;
            
            if (!_firstKeyboard && Keyboard.current.shiftKey.isPressed)
            {
                manager.JoinPlayer(_id, -1, "Keyboard1", Keyboard.current);
                print("Joined!");
                _firstKeyboard = true;
                _id++;
            }
            else if (!_secondKeyboard && Keyboard.current.enterKey.isPressed)
            {
                manager.JoinPlayer(_id, -1, "Keyboard2", Keyboard.current);
                _secondKeyboard = true;
                _id++;
            }
            else if (Gamepad.all.Count >= 1 && Gamepad.current.buttonSouth.isPressed && _deviceIds.All(n => !n.Equals(Gamepad.current.deviceId)))
            {
                _deviceIds.Add(Gamepad.current.deviceId);
                manager.JoinPlayer(_id, -1, "Controller", Gamepad.current);
                _id++;
            }
        }
    }
}
