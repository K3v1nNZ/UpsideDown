using System;
using UnityEngine;
using UpsideDown.Environment;
using UpsideDown.ScriptableObjects;

namespace UpsideDown.Player
{
    public class StatsManager : MonoBehaviour
    {
        public static StatsManager Instance;
        private int _enemiesKilled;
        private int _wavesSurvived;
        private int _towersPlaced;
        private float _timeSurvived;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Update()
        {
            if (!GridManager.Instance.isDead)
            {
                _timeSurvived += Time.deltaTime;
            }
        }

        public void EnemyKilled()
        {
            _enemiesKilled++;
        }

        public void WaveIncrease()
        {
            _wavesSurvived++;
        }

        public void TowerPlaced()
        {
            _towersPlaced++;
        }

        public GameOverStatsScriptableObject GetStats()
        {
            GameOverStatsScriptableObject gos = ScriptableObject.CreateInstance<GameOverStatsScriptableObject>();
            gos.enemiesKilled = _enemiesKilled;
            gos.wavesSurvived = _wavesSurvived;
            gos.towersPlaced = _towersPlaced;
            gos.TimeSurvived = TimeSpan.FromSeconds(_timeSurvived);
            return gos;
        }
    }
}