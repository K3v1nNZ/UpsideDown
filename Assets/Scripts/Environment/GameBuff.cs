using System;
using Object = UnityEngine.Object;

namespace UpsideDown.Environment
{
    [Serializable]
    public struct GameBuff
    {
        public string buffName;
        public Object buffScript;
        public string buffVariable;
        public BuffVarType buffVarType;
        public string buffMethod;
        public BuffPositivity buffPositivity;
    }

    public enum BuffPositivity
    {
        Positive,
        Negative,
        Neutral
    }

    public enum BuffVarType
    {
        Float,
        Int,
        Bool
    }
}