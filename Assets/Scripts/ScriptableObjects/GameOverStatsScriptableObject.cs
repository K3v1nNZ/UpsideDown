using System;
using UnityEngine;

namespace UpsideDown.ScriptableObjects
{
    public class GameOverStatsScriptableObject : ScriptableObject
    {
        public int enemiesKilled;
        public int wavesSurvived;
        public int towersPlaced;
        public TimeSpan TimeSurvived;
    }
}