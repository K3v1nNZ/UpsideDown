using System.Collections.Generic;
using UnityEngine;
using UpsideDown.Player;

namespace UpsideDown.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Storage", menuName = "Structures/Storage", order = 1)]
    public class StorageScriptableObject : StructureScriptableObject
    {
        public ResourceType resourceType;
        public List<int> upgradeStorageAmounts;
    }
}