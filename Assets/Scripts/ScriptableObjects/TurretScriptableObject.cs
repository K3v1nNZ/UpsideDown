using System.Collections.Generic;
using UnityEngine;

namespace UpsideDown.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Turret", menuName = "Structures/Turret", order = 1)]
    public class TurretScriptableObject : StructureScriptableObject
    {
        public int health;
        public float turretRange;
        public List<float> upgradeFireAmount;
        public List<int> upgradeDamageAmount;
    }
}