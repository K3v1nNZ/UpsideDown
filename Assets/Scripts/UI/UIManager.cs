using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpsideDown.Environment;
using UpsideDown.Player;
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
        [SerializeField] private TMP_Text playerResourcesStone;
        [SerializeField] private TMP_Text playerResourcesMetal;
        [SerializeField] private TMP_Text playerResourcesPower;
        
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

        private void Update()
        {
            playerResourcesStone.text = $"Stone: {ResourcesManager.Instance.playerResources.stone}";
            playerResourcesMetal.text = $"Metal: {ResourcesManager.Instance.playerResources.metal}";
            playerResourcesPower.text = $"Power: {ResourcesManager.Instance.playerResources.power}";
        }
        
        public void DestroyStructure()
        {
            GridManager.Instance.DestroyStructure();
            ToggleTowerUpgradePanel(false);
        }

        public void ToggleTowerUpgradePanel(bool state, Grid grid = null)
        {
            towerUpgradePanel.alpha = state ? 1 : 0;
            towerUpgradePanel.interactable = state;
            towerUpgradePanel.blocksRaycasts = state;
            if (!grid || !state) return;
            if (!grid.isOccupied)
            {
                towerUpgradeName.text = "Empty";
                towerUpgradeImage.texture = null;
                towerUpgradeLevel.text = "N/A";
                towerUpgradeUpgradeValue.text = "N/A";
                towerUpgradeDestroyButton.interactable = false;
                towerUpgradeUpgradeButton.interactable = false;
                return;
            }
            
            towerUpgradeName.text = grid.structure.structureName;
            towerUpgradeImage.texture = grid.structure.structureIcon;
            towerUpgradeDestroyButton.interactable = !grid.isCentreGrid;

            if (grid.isCentreGrid)
            {
                towerUpgradeUpgradeButton.interactable = false;
                towerUpgradeUpgradeValue.text = "N/A";
                towerUpgradeDestroyButton.interactable = false;
                return;
            }
            towerUpgradeLevel.text = grid.structureLevel.ToString();
            
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