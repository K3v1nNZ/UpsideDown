using System;
using UnityEngine;

namespace UpsideDown.Player
{
    [Serializable]
    public struct StructureUpgrade
    {
        public GameObject structurePrefab;
        public GameResources upgradeCost;
    }
}