using System.Collections.Generic;
using UnityEngine;

namespace UpsideDown.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Turret", menuName = "Structures/Turret", order = 1)]
    public class TurretScriptableObject : StructureScriptableObject
    {
        public Dictionary<float, int> UpgradeFirerateDamageAmounts;
    }
}