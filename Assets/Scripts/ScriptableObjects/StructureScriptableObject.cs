using System.Collections.Generic;
using UnityEngine;
using UpsideDown.Player;

namespace UpsideDown.ScriptableObjects
{
    public class StructureScriptableObject : ScriptableObject
    {
        public string structureName;
        public GameResources structureCost;
        public Dictionary<GameObject, GameResources> StructureUpgrades;
    }
}