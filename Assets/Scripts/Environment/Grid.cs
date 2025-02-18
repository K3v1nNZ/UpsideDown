using UnityEngine;
using UpsideDown.ScriptableObjects;

namespace UpsideDown.Environment
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private GameObject _gridHighlight;
        private StructureScriptableObject _structure;
        private bool _isHighlighted;
        public bool isOccupied;
        public bool isCentreGrid;
        
        public void CentreGrid()
        {
            // TODO: Add Core Structure to this grid tile
        }
        
        public void CreateStructure(StructureScriptableObject structure)
        {
            _structure = structure;
            isOccupied = true;
            GameObject structurePrefab = Instantiate(structure.structureUpgrades[0].structurePrefab, transform.position, Quaternion.identity);
            structurePrefab.transform.SetParent(transform);
        }
        
        public void GridSelection(bool state)
        {
            _isHighlighted = state;
            _gridHighlight.SetActive(state);
        }
    }
}
