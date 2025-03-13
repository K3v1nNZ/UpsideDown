using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UpsideDown.Environment
{
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager Instance;
        public int waveNumber;
        [SerializeField] private int wavesPerFlip;
        [SerializeField] private List<GameObject> enemyObjectsToSpawn = new();
        [SerializeField] private List<Transform> spawnPoints = new();
        [SerializeField] private float spawnIntervals;
        [SerializeField] private int startingEnemyCount;
        [SerializeField] private int increasingEnemyCount;
        private readonly List<GameObject> _aliveEnemies = new();
        private int _wavesPerFlipCounter;
        private bool _isWaveRunning = true;
        private float _waveCountdown;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (_isWaveRunning && _aliveEnemies.Count <= 0)
            {
                if (_waveCountdown > 20)
                {
                    _isWaveRunning = false;
                    _waveCountdown = 0;
                    StartCoroutine(WaveStart());
                }
                else
                {
                    _waveCountdown += Time.deltaTime;
                }
            }
        }

        public void EnemyDestroyed(GameObject enemy)
        {
            _aliveEnemies.Remove(enemy);
        }

        private IEnumerator WaveStart()
        {
            waveNumber++;
            _wavesPerFlipCounter++;
            if (_wavesPerFlipCounter > wavesPerFlip)
            {
                _wavesPerFlipCounter = 1;
                // im gonna flip
            }
            _isWaveRunning = true;
            int amountToSpawn = startingEnemyCount + (increasingEnemyCount * waveNumber);
            for (int i = 0; i < amountToSpawn; i++)
            {
                GameObject enemyToSpawn = enemyObjectsToSpawn[Random.Range(0, enemyObjectsToSpawn.Count)];
                GameObject enemy = Instantiate(enemyToSpawn, spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity);
                _aliveEnemies.Add(enemy);
                yield return new WaitForSeconds(spawnIntervals);
            }
        }
    }
}