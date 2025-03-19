using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UpsideDown.Player;
using UpsideDown.UI;
using Random = UnityEngine.Random;

namespace UpsideDown.Environment
{
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager Instance;
        public int waveNumber;
        [SerializeField] private GameObject mapToFlip;
        [SerializeField] private int wavesPerFlip;
        [SerializeField] private List<GameObject> enemyObjectsToSpawn = new();
        [SerializeField] private List<Transform> spawnPoints = new();
        [SerializeField] private float spawnIntervals;
        [SerializeField] private int startingEnemyCount;
        [SerializeField] private int increasingEnemyCount;
        [SerializeField] private List<GameObject> maps = new();
        private readonly List<GameObject> _aliveEnemies = new();
        private int _wavesPerFlipCounter;
        private bool _isWaveRunning = true;
        private float _enemySpeedBuff = 1;
        private float _enemyDamageBuff = 1;
        private float _enemyHealthBuff = 1;
        public float waveCountdown;
        
        public delegate void WaveEnd();
        public static event WaveEnd OnWaveEnd;

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

        private void Start()
        {
            FlipLogic(true);
        }

        private void Update()
        {
            if (_isWaveRunning && _aliveEnemies.Count <= 0 && !GridManager.Instance.isDead)
            {
                UIManager.Instance.SetWaveCountdownVisibility(true);
                if (waveCountdown <= 0)
                {
                    UIManager.Instance.SetWaveCountdownVisibility(false);
                    _isWaveRunning = false;
                    waveCountdown = 20;
                    OnWaveEnd?.Invoke();
                    StartCoroutine(WaveStart());
                }
                else
                {
                    waveCountdown -= Time.deltaTime;
                }
            }
        }

        public void EnemySpeedBuff(float dividedBy)
        {
            _enemySpeedBuff = dividedBy;
        }

        public void EnemyDamageBuff(float dividedBy)
        {
            _enemyDamageBuff = dividedBy;
        }

        public void EnemyHealthBuff(float dividedBy)
        {
            _enemyHealthBuff = dividedBy;
        }

        public void EnemyDestroyed(GameObject enemy)
        {
            _aliveEnemies.Remove(enemy);
        }

        private IEnumerator WaveStart()
        {
            waveNumber++;
            _wavesPerFlipCounter++;
            StatsManager.Instance.WaveIncrease();
            if (_wavesPerFlipCounter > wavesPerFlip)
            {
                _wavesPerFlipCounter = 1;
                CameraController.Instance.FlipCam(true);
                UIManager.Instance.SetUIVisibility(false);
                StructureCreator.Instance.canPlaceStructure = false;
                StructureCreator.Instance.CancelPlacement();
                yield return new WaitForSeconds(1f);
                mapToFlip.transform.DORotate(new Vector3(180, 0, 0), 1f);
                yield return new WaitForSeconds(1f);
                FlipLogic(false);
                mapToFlip.transform.DORotate(new Vector3(360, 0, 0), 1f);
                yield return new WaitForSeconds(1f);
                mapToFlip.transform.rotation = Quaternion.identity;
                CameraController.Instance.FlipCam(false);
                UIManager.Instance.SetUIVisibility(true);
                StructureCreator.Instance.canPlaceStructure = true;
                yield return new WaitForSeconds(0.25f);
                UIManager.Instance.SetFlipStatsVisibility();
            }

            if (waveNumber is > 3 and < 7)
            {
                spawnIntervals = 2.5f;
            }
            else if (waveNumber is > 6 and < 10)
            {
                spawnIntervals = 2;
            }
            else if (waveNumber > 9)
            {
                spawnIntervals = 1.5f;
            }
            else
            {
                spawnIntervals = 3;
            }
            
            _isWaveRunning = true;
            int amountToSpawn = startingEnemyCount + (increasingEnemyCount * waveNumber);
            for (int i = 0; i < amountToSpawn; i++)
            {
                if (GridManager.Instance.isDead) yield break;
                GameObject enemyToSpawn = enemyObjectsToSpawn[Random.Range(0, enemyObjectsToSpawn.Count)];
                GameObject enemy = Instantiate(enemyToSpawn, spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity);
                _aliveEnemies.Add(enemy);
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                enemyComponent.BuffSpeed(_enemySpeedBuff);
                enemyComponent.BuffDamage(_enemyDamageBuff);
                enemyComponent.BuffHealth(_enemyHealthBuff);
                yield return new WaitForSeconds(spawnIntervals);
            }
        }

        public void KillThemAll()
        {
            foreach (GameObject enemy in _aliveEnemies.ToList())
            {
                _aliveEnemies.Remove(enemy);
                Destroy(enemy);
            }
        }

        private void FlipLogic(bool isStart)
        {
            foreach (GameObject map in maps)
            {
                map.SetActive(false);
            }

            int mapNumber = Random.Range(0, maps.Count);
            if (isStart) mapNumber = 0;
            maps[mapNumber].SetActive(true);
            
            BuffManager.Instance.FlipBuffChange(mapNumber);
        }
    }
}