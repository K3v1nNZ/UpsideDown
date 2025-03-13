using System.Collections.Generic;
using UnityEngine;
using UpsideDown.Player;

namespace UpsideDown.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Base Structure", menuName = "Structures/(Core) Base Structure")]
    public class StructureScriptableObject : ScriptableObject
    {
        public string structureName;
        public Texture2D structureIcon;
        public List<StructureUpgrade> structureUpgrades;
        public StructureType structureType;
        
        public enum StructureType
        {
            Storage,
            Generator,
            Turret,
            Wall,
            Other
        }

        public virtual void StructureStart(int level) {}
        
        public virtual void StructureStop(int level) {}
    }
}