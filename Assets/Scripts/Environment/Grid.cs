using UnityEngine;
using UnityEngine.Serialization;
using UpsideDown.ScriptableObjects;

namespace UpsideDown.Environment
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private GameObject _gridHighlight;
        [HideInInspector] public StructureScriptableObject structure;
        private bool _isHighlighted;
        public bool isOccupied;
        public bool isCentreGrid;
        public int structureLevel;
        
        public void CentreGrid()
        {
            // TODO: Add Core Structure to this grid tile
        }
        
        public void CreateStructure(StructureScriptableObject structure)
        {
            this.structure = structure;
            isOccupied = true;
            GameObject structurePrefab = Instantiate(structure.structureUpgrades[0].structurePrefab, transform.position, Quaternion.identity);
            structurePrefab.transform.SetParent(transform);
            structureLevel = 1;
        }
        
        public void GridSelection(bool state)
        {
            _isHighlighted = state;
            _gridHighlight.SetActive(state);
        }
    }
}
