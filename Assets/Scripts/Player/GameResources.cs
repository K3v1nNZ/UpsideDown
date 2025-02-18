using System;

namespace UpsideDown.Player
{
    [Serializable]
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
        
        public GameResources(int stone, int metal, int power)
        {
            this.stone = stone;
            this.metal = metal;
            this.power = power;
        }
        
        public bool CanAfford(GameResources cost)
        {
            return stone >= cost.stone && metal >= cost.metal && power >= cost.power;
        }
        
        public bool Deduct(GameResources cost)
        {
            if (!CanAfford(cost))
            {
                return false;
            }
            
            stone -= cost.stone;
            metal -= cost.metal;
            power -= cost.power;
            return true;
        }
        
        public void Add(GameResources resources)
        {
            stone += resources.stone;
            metal += resources.metal;
            power += resources.power;
        }
    }
}