using System.Collections.Generic;
using UnityEngine;

namespace UpsideDown.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Wall", menuName = "Structures/Wall", order = 1)]
    public class WallScriptableObject : StructureScriptableObject
    {
        public List<int> wallHealth;
    }
}