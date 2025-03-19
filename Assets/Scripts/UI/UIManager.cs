using System;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
        [SerializeField] private CanvasGroup whiteFade;
        [SerializeField] private CanvasGroup blackFade;
        [SerializeField] private CanvasGroup mainCanvasGroup;
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
        [SerializeField] private Image healthBarFill;
        [SerializeField] private TMP_Text playerResourcesStone;
        [SerializeField] private TMP_Text playerResourcesMetal;
        [SerializeField] private TMP_Text playerResourcesPower;
        [SerializeField] private TMP_Text waveCounter;
        [SerializeField] private TMP_Text waveTimer;
        [SerializeField] private RectTransform waveTimerPanel;
        [SerializeField] private RectTransform flipStatsPanel;
        [SerializeField] private GameObject flipStatsContainer;
        [SerializeField] private GameObject flipStatsObject;
        [SerializeField] private CanvasGroup gameOverPanel;
        [SerializeField] private TMP_Text timeSurvivedText;
        [SerializeField] private TMP_Text wavesSurvivedText;
        [SerializeField] private TMP_Text enemiesKilledText;
        [SerializeField] private TMP_Text towersPlacedText;
        [HideInInspector] public Grid grid;
        private bool _waveTimerVisibility;
        private bool _flipStatsPanelVisibility;
        
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
            whiteFade.DOFade(0, 0.5f);
        }

        private void Update()
        {
            playerResourcesStone.text = $"Stone: {ResourcesManager.Instance.playerResources.stone}/{ResourcesManager.Instance.playerResources.StoneLimit}";
            playerResourcesMetal.text = $"Metal: {ResourcesManager.Instance.playerResources.metal}/{ResourcesManager.Instance.playerResources.MetalLimit}";
            playerResourcesPower.text = $"Power: {ResourcesManager.Instance.playerResources.power}/{ResourcesManager.Instance.playerResources.PowerLimit}";
            waveCounter.text = $"Wave: {WaveManager.Instance.waveNumber}";
            waveTimer.text = WaveManager.Instance.waveCountdown.ToString("0.00");

            if (grid)
            {
                healthBarFill.fillAmount = (float)grid.health / grid.maxHealth;
            }
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
            this.grid = grid;
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

        public void SetUIVisibility(bool state)
        {
            mainCanvasGroup.DOFade(state ? 1 : 0, 0.25f);
            mainCanvasGroup.interactable = state;
            mainCanvasGroup.blocksRaycasts = state;
        }

        public void SetWaveCountdownVisibility(bool state)
        {
            if (state == _waveTimerVisibility) return;
            waveTimerPanel.DOAnchorPosX(state ? 0 : 130, 0.5f).SetEase(Ease.OutQuint);
            _waveTimerVisibility = state;
        }

        public void ClearFlipStats()
        {
            foreach (Transform child in flipStatsContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void SetFlipStat(string title, string value, FlipStatsObject.ColourState colourState)
        {
            GameObject flipStat = Instantiate(flipStatsObject, flipStatsContainer.transform);
            flipStat.GetComponent<FlipStatsObject>().Setup(title, value, colourState);
        }

        public void SetFlipStatsVisibility()
        {
            _flipStatsPanelVisibility = !_flipStatsPanelVisibility;
            flipStatsPanel.DOAnchorPosY(_flipStatsPanelVisibility ? -150 : 650, 0.3f).SetEase(Ease.OutQuint);
        }

        public void ShowGameOverScreen()
        {
            GameOverStatsScriptableObject so = StatsManager.Instance.GetStats();
            timeSurvivedText.text = so.TimeSurvived.ToString(@"hh\:mm\:ss");
            wavesSurvivedText.text = so.wavesSurvived.ToString();
            enemiesKilledText.text = so.enemiesKilled.ToString();
            towersPlacedText.text = so.towersPlaced.ToString();
            gameOverPanel.DOFade(1f, 0.3f).OnComplete((() =>
            {
                gameOverPanel.interactable = true;
                gameOverPanel.blocksRaycasts = true;
            }));
        }

        public void MainMenuButton()
        {
            AsyncOperation sceneAsync = SceneManager.LoadSceneAsync("MainMenu");
            sceneAsync.allowSceneActivation = false;
            blackFade.DOFade(1, 0.5f).OnComplete(() => sceneAsync.allowSceneActivation = true); 
        }
    }
}