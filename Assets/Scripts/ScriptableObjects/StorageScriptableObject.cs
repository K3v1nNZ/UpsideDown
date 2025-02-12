using System.Collections.Generic;
using UpsideDown.Player;

namespace UpsideDown.ScriptableObjects
{
    public class StorageScriptableObject : StructureScriptableObject
    {
        public ResourceType resourceType;
        public List<int> upgradeStorageAmounts;
    }
}