using System;
using System.Collections;
using System.Collections.Generic;
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
            if (_isWaveRunning && _aliveEnemies.Count <= 0)
            {
                UIManager.Instance.SetWaveCountdownVisibility(true);
                if (waveCountdown <= 0)
                {
                    UIManager.Instance.SetWaveCountdownVisibility(false);
                    _isWaveRunning = false;
                    waveCountdown = 20;
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
            _isWaveRunning = true;
            int amountToSpawn = startingEnemyCount + (increasingEnemyCount * waveNumber);
            for (int i = 0; i < amountToSpawn; i++)
            {
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