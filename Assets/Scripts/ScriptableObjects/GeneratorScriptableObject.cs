using System.Collections.Generic;
using UpsideDown.Player;

namespace UpsideDown.ScriptableObjects
{
    public class GeneratorScriptableObject : StructureScriptableObject
    {
        public ResourceType resourceType;
        public List<int> upgradeGenerationAmounts;
    }
}