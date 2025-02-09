using UnityEngine;

namespace UpsideDown.Environment
{
    // GridManager is a script that runs during runtime to create
    // and manage the grid layout for the playable area of the game.
    // The interactability of the grid is determined by the Grid script.
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private int gridWidthX;
        [SerializeField] private int gridWidthZ;
        [SerializeField] private GameObject gridPrefab;
        [SerializeField] private GameObject gridParent;
        
        private void Start()
        {
            CreateGrid();
        }
        
        private void CreateGrid()
        {
            for (int x = 0; x < gridWidthX; x++)
            {
                for (int z = 0; z < gridWidthZ; z++)
                {
                    GameObject grid = Instantiate(gridPrefab, new Vector3(x, 0, z), Quaternion.identity);
                    grid.transform.SetParent(gridParent.transform);
                }
            }
        }
    }
}