using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Grid = UpsideDown.Environment.Grid;

namespace UpsideDown.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        [SerializeField] private CanvasGroup towerUpgradePanel;
        [SerializeField] private TMP_Text towerUpgradeName;
        [SerializeField] private RawImage towerUpgradeImage;
        [SerializeField] private Button towerUpgradeDestroyButton;
        [SerializeField] private Button towerUpgradeUpgradeButton;
        [SerializeField] private TMP_Text towerUpgradeUpgradeValue;
        [SerializeField] private TMP_Text towerUpgradeLevel;

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
        
        public void ToggleTowerUpgradePanel(bool state, Grid grid = null)
        {
            towerUpgradePanel.alpha = state ? 1 : 0;
            towerUpgradePanel.interactable = state;
            towerUpgradePanel.blocksRaycasts = state;
            if (!grid || !state) return;

            towerUpgradeName.text = grid.structure.structureName;
            towerUpgradeImage.texture = grid.structure.structureIcon;
            towerUpgradeDestroyButton.interactable = !grid.isCentreGrid;
            towerUpgradeLevel.text = grid.structureLevel.ToString();

            if (grid.isCentreGrid)
            {
                towerUpgradeUpgradeButton.interactable = false;
                towerUpgradeUpgradeValue.text = "N/A";
                towerUpgradeDestroyButton.interactable = false;
                return;
            }
            
            if (grid.structureLevel >= grid.structure.structureUpgrades.Count)
            {
                towerUpgradeUpgradeButton.interactable = false;
                towerUpgradeUpgradeValue.text = "Max";
            }
            else
            {
                towerUpgradeUpgradeButton.interactable = grid.structure.structureUpgrades.Count > grid.structureLevel;
                towerUpgradeUpgradeValue.text = grid.structure.structureUpgrades[grid.structureLevel].upgradeCost.ToString();
            }
        }
    }
}