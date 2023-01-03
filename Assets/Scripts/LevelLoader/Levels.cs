using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helicoopter
{
    [CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
    public class Levels : ScriptableObject
    {
        public bool isLocked;
        public bool isBeaten;
        public string levelName;
    }
}
