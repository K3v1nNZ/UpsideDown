using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpsideDown.Environment;
using UpsideDown.Player;
using UpsideDown.ScriptableObjects;
using Grid = UpsideDown.Environment.Grid;

namespace UpsideDown.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        [SerializeField] private CanvasGroup towerPurchase;
        [SerializeField] private Color tabSelectedColor;
        [SerializeField] private Color tabUnselectedColor;
        [SerializeField] private CanvasGroup defenceTab;
        [SerializeField] private Image defenceTabButton;
        [SerializeField] private CanvasGroup generatorTab;
        [SerializeField] private Image generatorTabButton;
        [SerializeField] private CanvasGroup storageTab;
        [SerializeField] private Image storageTabButton;
        [SerializeField] private CanvasGroup towerPlacementPanel;
        [SerializeField] private TMP_Text stonePlacementCost;
        [SerializeField] private TMP_Text metalPlacementCost;
        [SerializeField] private TMP_Text powerPlacementCost;
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
            GridManager.Instance.SelectGrid(null);
        }

        public void UpgradeStructure()
        {
            GridManager.Instance.UpgradeStructure();
            GridManager.Instance.SelectGrid(null);
        }

        public void SwitchTab(int tab)
        {
            defenceTab.DOFade(tab == 0 ? 1 : 0, 0.25f);
            defenceTab.interactable = tab == 0;
            defenceTab.blocksRaycasts = tab == 0;
            defenceTabButton.DOColor(tab == 0 ? tabSelectedColor : tabUnselectedColor, 0.25f);
            
            generatorTab.DOFade(tab == 1 ? 1 : 0, 0.25f);
            generatorTab.interactable = tab == 1;
            generatorTab.blocksRaycasts = tab == 1;
            generatorTabButton.DOColor(tab == 1 ? tabSelectedColor : tabUnselectedColor, 0.25f);
            
            storageTab.DOFade(tab == 2 ? 1 : 0, 0.25f);
            storageTab.interactable = tab == 2;
            storageTab.blocksRaycasts = tab == 2;
            storageTabButton.DOColor(tab == 2 ? tabSelectedColor : tabUnselectedColor, 0.25f);
        }

        public async Task PlacementUI(StructureScriptableObject structure)
        {
            RectTransform panel = towerPlacementPanel.transform as RectTransform;
            if (structure)
            {
                towerPurchase.interactable = false;
                
                stonePlacementCost.text = structure.structureUpgrades[0].upgradeCost.stone.ToString();
                metalPlacementCost.text = structure.structureUpgrades[0].upgradeCost.metal.ToString();
                powerPlacementCost.text = structure.structureUpgrades[0].upgradeCost.power.ToString();

                panel.position = new Vector3(panel.position.x, -100f);
                towerPlacementPanel.alpha = 1;
                await panel.DOAnchorPosY(100, 0.5f).SetEase(Ease.OutQuint).AsyncWaitForCompletion();
                towerPlacementPanel.interactable = true;
                towerPlacementPanel.blocksRaycasts = true;
            }
            else
            {
                towerPlacementPanel.interactable = false;
                towerPlacementPanel.blocksRaycasts = false;
                await panel.DOAnchorPosY(-50, 0.5f).SetEase(Ease.OutQuint).AsyncWaitForCompletion();
                towerPlacementPanel.alpha = 0;
                
                towerPurchase.interactable = true;
            }
        }
        
        public void CancelPlacement()
        {
            StructureCreator.Instance.CancelPlacement();
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
                GameResources resources = grid.structure.structureUpgrades[grid.structureLevel].upgradeCost;
                towerUpgradeUpgradeValue.text = $"Stone:{resources.stone} Metal:{resources.metal} Power:{resources.power}";
            }
        }
    }
}