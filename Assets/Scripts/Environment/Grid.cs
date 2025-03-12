using UnityEngine;
using UpsideDown.Player;
using UpsideDown.ScriptableObjects;

namespace UpsideDown.Environment
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private GameObject _gridHighlight;
        [HideInInspector] public StructureScriptableObject structure;
        public int health;
        public bool isOccupied;
        public bool isCentreGrid;
        public bool isEdge;
        public int structureLevel;
        public StructureScriptableObject.StructureType structureType;
        private float _generatorTimer;

        private void Update()
        {
            if (!isOccupied || structureType == StructureScriptableObject.StructureType.Other || structureType == StructureScriptableObject.StructureType.Wall) return;
            
            switch (structureType)
            {
                case StructureScriptableObject.StructureType.Generator:
                    GeneratorUpdate();
                    break;
                case StructureScriptableObject.StructureType.Turret:
                    TurretUpdate();
                    break;
                case StructureScriptableObject.StructureType.Storage:
                    StorageUpdate();
                    break;
            }
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
        
        private void TurretUpdate()
        {
            
        }
        
        private void StorageUpdate()
        {
            
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
            health = 100;
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
            health = 100;
        }

        public void DestroyStructure()
        {
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
            _gridHighlight.SetActive(state);
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

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                DestroyStructure();
            }
        }
    }
}