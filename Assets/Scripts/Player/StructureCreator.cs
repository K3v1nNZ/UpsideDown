using UnityEngine;
using UpsideDown.ScriptableObjects;
using Grid = UpsideDown.Environment.Grid;

namespace UpsideDown.Player
{
    public class StructureCreator : MonoBehaviour
    {
        [SerializeField] private GameObject creatorMousePosition;
        [SerializeField] private LayerMask gridLayerMask;
        private StructureScriptableObject _structure;
        private GameObject _hoveredGrid;
        private Vector3 _objectHoldPos;
        private InputActions _inputActions;
        public bool isPlacingStructure;

        private void Start()
        {
            _inputActions = PlayerInputManager.Instance.PlayerInputActions;
        }

        private void Update()
        {
            if (!isPlacingStructure) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 300f, gridLayerMask))
            {
                if (hit.transform.gameObject != _hoveredGrid)
                {
                    _hoveredGrid = hit.transform.gameObject;
                    creatorMousePosition.transform.position = hit.transform.position;
                    _objectHoldPos = hit.transform.position;
                }
                creatorMousePosition.transform.position = _objectHoldPos;
            }
            
            if (_inputActions.Player.Select.WasPressedThisFrame())
            {
                PlaceStructure();
            }
        }

        private void PlaceStructure()
        {
            if (!_hoveredGrid) return;
            if (_hoveredGrid.transform.parent.gameObject.GetComponent<Grid>().isOccupied) return;
            if (!ResourcesManager.Instance.playerResources.CanAfford(_structure.structureUpgrades[0].upgradeCost)) return;
            ResourcesManager.Instance.playerResources.Deduct(_structure.structureUpgrades[0].upgradeCost);
            _hoveredGrid.transform.parent.gameObject.GetComponent<Grid>().CreateStructure(_structure);
            isPlacingStructure = false;
            _hoveredGrid = null;
        }

        public void CreateStructure(StructureScriptableObject structure)
        {
            if (isPlacingStructure) return;
            _structure = structure;
            GameObject structurePrefab = Instantiate(structure.structureUpgrades[0].structurePrefab, creatorMousePosition.transform.position, Quaternion.identity);
            structurePrefab.transform.SetParent(creatorMousePosition.transform);
            isPlacingStructure = true;
        }
    }
}