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

        public override void StructureStart(int level)
        {
            ResourcesManager.Instance.playerResources.AddLimit(resourceType, upgradeStorageAmounts[level - 1]);
        }

        public override void StructureStop(int level)
        {
            ResourcesManager.Instance.playerResources.DeductLimit(resourceType, upgradeStorageAmounts[level - 1]);
        }
    }
}