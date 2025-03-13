using System.Collections.Generic;
using UnityEngine;
using UpsideDown.Player;

namespace UpsideDown.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Generator", menuName = "Structures/Generator", order = 1)]
    public class GeneratorScriptableObject : StructureScriptableObject
    {
        public int health;
        public ResourceType resourceType;
        public List<int> upgradeGenerationAmounts;
    }
}