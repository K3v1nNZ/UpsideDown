using System;
using UnityEngine.Serialization;

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
        public int StoneLimit { get; private set; }
        public int MetalLimit { get; private set; }
        public int PowerLimit { get; private set; }
        
        public GameResources(int stone, int metal, int power)
        {
            this.stone = stone;
            this.metal = metal;
            this.power = power;
            StoneLimit = 50;
            MetalLimit = 50;
            PowerLimit = 50;
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
            
            CheckLimits();
            return true;
        }
        
        public void Add(GameResources resources)
        {
            stone += resources.stone;
            metal += resources.metal;
            power += resources.power;

            CheckLimits();
        }

        public void AddLimit(ResourceType resourceType, int amount)
        {
            switch (resourceType)
            {
                case ResourceType.Stone:
                    StoneLimit += amount;
                    break;
                case ResourceType.Metal:
                    MetalLimit += amount;
                    break;
                case ResourceType.Power:
                    PowerLimit += amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
            }
            
            CheckLimits();
        }

        public void DeductLimit(ResourceType resourceType, int amount)
        {
            switch (resourceType)
            {
                case ResourceType.Stone:
                    StoneLimit -= amount;
                    break;
                case ResourceType.Metal:
                    MetalLimit -= amount;
                    break;
                case ResourceType.Power:
                    PowerLimit -= amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
            }
            
            CheckLimits();
        }
        
        private void CheckLimits()
        {
            if (stone > StoneLimit) stone = StoneLimit;
            if (metal > MetalLimit) metal = MetalLimit;
            if (power > PowerLimit) power = PowerLimit;
        }
    }
}