using System.Collections.Generic;
using UnityEngine;
using UpsideDown.Player;

namespace UpsideDown.ScriptableObjects
{
    public class StructureScriptableObject : ScriptableObject
    {
        public string structureName;
        public Texture2D structureIcon;
        public List<StructureUpgrade> structureUpgrades;
    }
}