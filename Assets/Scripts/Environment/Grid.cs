using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UpsideDown.Player;
using UpsideDown.ScriptableObjects;
using UpsideDown.UI;

namespace UpsideDown.Environment
{
    public class Grid : MonoBehaviour
    {
        [FormerlySerializedAs("_gridHighlight")] [SerializeField] private GameObject gridHighlight;
        [HideInInspector] public StructureScriptableObject structure;
        [SerializeField] public LayerMask turretLayerMask;
        public int health;
        public int maxHealth;
        public bool isOccupied;
        public bool isCentreGrid;
        public bool isEdge;
        public int structureLevel;
        public StructureScriptableObject.StructureType structureType;
        private float _generatorTimer;
        private float _turretTimer;

        private void OnEnable()
        {
            WaveManager.OnWaveEnd += WaveEnd;
        }

        private void OnDisable()
        {
            WaveManager.OnWaveEnd -= WaveEnd;
        }

        private void Update()
        {
            if (!isOccupied || structureType == StructureScriptableObject.StructureType.Other || structureType == StructureScriptableObject.StructureType.Wall || structureType == StructureScriptableObject.StructureType.Storage) return;

            switch (structureType)
            {
                case StructureScriptableObject.StructureType.Generator:
                    GeneratorUpdate();
                    break;
                case StructureScriptableObject.StructureType.Turret:
                    TurretUpdate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void WaveEnd()
        {
            if (!isCentreGrid) health = maxHealth;
        }

        private void GeneratorUpdate()
        {
            if (_generatorTimer >= 10)
            {
                _generatorTimer = 0;
                GeneratorScriptableObject generator = structure as GeneratorScriptableObject;
                switch (generator.resourceType)
                {
                    case ResourceType.Stone:
                        ResourcesManager.Instance.playerResources.Add(new GameResources(generator.upgradeGenerationAmounts[structureLevel - 1], 0, 0));
                        break;
                    case ResourceType.Metal:
                        ResourcesManager.Instance.playerResources.Add(new GameResources(0, generator.upgradeGenerationAmounts[structureLevel - 1], 0));
                        break;
                    case ResourceType.Power:
                        ResourcesManager.Instance.playerResources.Add(new GameResources(0, 0, generator.upgradeGenerationAmounts[structureLevel - 1]));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                _generatorTimer += Time.deltaTime;
            }
        }

        /*private void OnDrawGizmos()
        {
            if (structureType == StructureScriptableObject.StructureType.Turret)
            {
                TurretScriptableObject turret = structure as TurretScriptableObject;
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(transform.position, turret.turretRange);
            }
        }*/

        private void TurretUpdate()
        {
            TurretScriptableObject turret = structure as TurretScriptableObject;
            if (_turretTimer >= turret.upgradeFireAmount[structureLevel - 1])
            {
                _turretTimer = 0;
                Collider[] results = new Collider[10];
                int a = Physics.OverlapSphereNonAlloc(transform.position, turret.turretRange, results, turretLayerMask);
                if (a == 0) return;
                Collider closestEnemy = results[0];
                foreach (Collider enemy in results)
                {
                    if (!enemy) continue;
                    float current = Vector3.Distance(transform.position, closestEnemy.transform.position);
                    float loop = Vector3.Distance(transform.position, enemy.transform.position);
                    if (current >= loop)
                    {
                        closestEnemy = enemy;
                    }
                }
                closestEnemy.gameObject.GetComponent<Enemy>().TakeDamage(turret.upgradeDamageAmount[structureLevel - 1]);
                foreach (Transform child in transform)
                {
                    if (child.CompareTag("GridModel"))
                    {
                        foreach (Transform pivotChild in child.transform)
                        {
                            if (pivotChild.CompareTag("TurretPivot"))
                            {
                                pivotChild.DOLookAt(new Vector3(closestEnemy.transform.position.x, pivotChild.position.y, closestEnemy.transform.position.z), 0.05f).SetEase(Ease.Linear).OnComplete(() => pivotChild.DOShakeScale(0.3f, new Vector3(0, 0, -0.2f)));
                            }
                        }
                    }
                }
            }
            else
            {
                _turretTimer += Time.deltaTime;
            }
        }

        public void CentreGrid(StructureScriptableObject coreStructure)
        { 
            if (isEdge) return;
            structure = coreStructure;
            isOccupied = true;
            isCentreGrid = true;
            GameObject structurePrefab = Instantiate(coreStructure.structureUpgrades[0].structurePrefab, transform.position, Quaternion.identity);
            structurePrefab.transform.SetParent(transform);
            structureLevel = 1;
            structureType = coreStructure.structureType;
            SetHealth();
        }
        
        public void CreateStructure(StructureScriptableObject structure, int level, bool upgrade = false)
        {
            this.structure = structure;
            isOccupied = true;
            GameObject structurePrefab = Instantiate(structure.structureUpgrades[upgrade ? level : 0].structurePrefab, transform.position, Quaternion.identity);
            if (structure.structureType == StructureScriptableObject.StructureType.Wall)
            {
                structurePrefab.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            }
            structurePrefab.transform.SetParent(transform);
            structureLevel = upgrade ? level + 1 : 1;
            structureType = structure.structureType;
            structure.StructureStart(structureLevel);
            GridManager.Instance.UpdateNavMeshAsync();
            SetHealth();
        }

        public void DestroyStructure()
        {
            if (!structure) return;
            foreach (Transform child in transform)
            {
                if (child.CompareTag("GridModel") || child.CompareTag("EdgeModel"))
                {
                    Destroy(child.gameObject);
                }
            }
            structure.StructureStop(structureLevel);
            structure = null;
            isOccupied = false;
            structureLevel = 0;
            structureType = StructureScriptableObject.StructureType.Other;
            GridManager.Instance.UpdateNavMeshAsync();
        }
        
        public void GridSelection(bool state)
        {
            gridHighlight.SetActive(state);
        }

        public void Upgrade()
        {
            if (ResourcesManager.Instance.playerResources.CanAfford(structure.structureUpgrades[structureLevel].upgradeCost))
            {
                ResourcesManager.Instance.playerResources.Deduct(structure.structureUpgrades[structureLevel].upgradeCost);
                StructureScriptableObject tower = structure;
                int currentLevel = structureLevel;
                DestroyStructure();
                CreateStructure(tower, currentLevel, true);
            }
        }

        private void SetHealth()
        {
            switch (structureType)
            {
                case StructureScriptableObject.StructureType.Generator:
                    GeneratorScriptableObject generatorScriptableObject = structure as GeneratorScriptableObject;
                    health = generatorScriptableObject.health;
                    maxHealth = generatorScriptableObject.health;
                    break;
                case StructureScriptableObject.StructureType.Turret:
                    TurretScriptableObject turretScriptableObject = structure as TurretScriptableObject;
                    health = turretScriptableObject.health;
                    maxHealth = turretScriptableObject.health;
                    break;
                case StructureScriptableObject.StructureType.Storage:
                    StorageScriptableObject storageScriptableObject = structure as StorageScriptableObject;
                    health = storageScriptableObject.health;
                    maxHealth = storageScriptableObject.health;
                    break;
                case StructureScriptableObject.StructureType.Wall:
                    WallScriptableObject wallScriptableObject = structure as WallScriptableObject;
                    health = wallScriptableObject.wallHealth[structureLevel - 1];
                    maxHealth = wallScriptableObject.wallHealth[structureLevel - 1];
                    break;
                case StructureScriptableObject.StructureType.Other:
                    health = 100;
                    maxHealth = 100;
                    break;
                default:
                    break;
            }
        }
        
        public void TakeDamage(int damage)
        {
            health -= damage;
            foreach (Transform child in transform)
            {
                if (child.CompareTag("GridModel") || child.CompareTag("EdgeModel"))
                {
                    child.DOPunchScale(Vector3.zero, 0.4f, elasticity:0.2f);
                }
            }
            if (health <= 0)
            {
                if (UIManager.Instance.grid == this) UIManager.Instance.ToggleTowerUpgradePanel(false);

                if (isCentreGrid)
                {
                    UIManager.Instance.SetUIVisibility(false);
                    StartCoroutine(GridManager.Instance.EndGame());
                    return;
                }
                DestroyStructure();
            }
        }
    }
}