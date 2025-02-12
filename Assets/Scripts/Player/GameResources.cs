using System;

namespace UpsideDown.Player
{
    public enum ResourceType
    {
        Stone,
        Metal,
        Power
    }
    
    [Serializable]
    public struct GameResources
    {
        public int stone;
        public int metal;
        public int power;
    }
}