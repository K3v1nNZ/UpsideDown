using UnityEngine;

namespace UpsideDown.Environment
{
    // GridManager is a script that runs during runtime to create
    // and manage the grid layout for the playable area of the game.
    // The interactability of the grid is determined by the Grid script.
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;
        [HideInInspector] public Grid centreGrid;
        [SerializeField] private int gridWidthX;
        [SerializeField] private int gridWidthZ;
        [SerializeField] private GameObject gridPrefab;
        [SerializeField] private GameObject gridParent;
        private Grid _selectedGrid;

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
            CreateGrid();
        }

        private void CreateGrid()
        {
            if (gridWidthX % 2 == 0 || gridWidthZ % 2 == 0)
            {
                Debug.LogError("Grid width X and Z must be odd numbers.");
                return;
            }
            
            for (int x = 0; x < gridWidthX; x++)
            {
                for (int z = 0; z < gridWidthZ; z++)
                {
                    GameObject grid = Instantiate(gridPrefab, new Vector3(x - gridWidthX / 2, 0, z - gridWidthZ / 2), Quaternion.identity);
                    grid.transform.SetParent(gridParent.transform);
                    if (x == gridWidthX / 2 && z == gridWidthZ / 2)
                    {
                        centreGrid = grid.GetComponent<Grid>();
                        centreGrid.CentreGrid();
                    }
                }
            }
        }

        public void SelectGrid(Grid grid)
        {
            if (_selectedGrid != null)
            {
                _selectedGrid.GridSelection(false);
            }
            if (grid != null)
            {
                _selectedGrid = grid;
                _selectedGrid.GridSelection(true);
            }
        }
    }
}