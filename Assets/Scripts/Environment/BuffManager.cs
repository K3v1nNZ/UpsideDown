using UnityEngine;
using UpsideDown.UI;

namespace UpsideDown.Environment
{
    public class BuffManager : MonoBehaviour
    {
        public static BuffManager Instance;
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

        public void FlipBuffChange(int mapNumber)
        {
            switch (mapNumber)
            {
                case 1:
                    
                    WaveManager.Instance.EnemySpeedBuff(0.5f);
                    WaveManager.Instance.EnemyDamageBuff(0.5f);
                    WaveManager.Instance.EnemyHealthBuff(2f);
                    UIManager.Instance.ClearFlipStats();
                    UIManager.Instance.SetFlipStat("Enemy Speed", "200%", FlipStatsObject.ColourState.Negative);
                    UIManager.Instance.SetFlipStat("Enemy Damage", "200%", FlipStatsObject.ColourState.Negative);
                    UIManager.Instance.SetFlipStat("Enemy Health", "50%", FlipStatsObject.ColourState.Positive);
                    break;
                case 2:
                    WaveManager.Instance.EnemySpeedBuff(2);
                    WaveManager.Instance.EnemyDamageBuff(1.5f);
                    WaveManager.Instance.EnemyHealthBuff(0.5f);
                    UIManager.Instance.ClearFlipStats();
                    UIManager.Instance.SetFlipStat("Enemy Speed", "50%", FlipStatsObject.ColourState.Positive);
                    UIManager.Instance.SetFlipStat("Enemy Damage", "66%", FlipStatsObject.ColourState.Positive);
                    UIManager.Instance.SetFlipStat("Enemy Health", "200%", FlipStatsObject.ColourState.Negative);
                    break;
                case 3:
                    WaveManager.Instance.EnemySpeedBuff(0.5f);
                    WaveManager.Instance.EnemyDamageBuff(1.5f);
                    WaveManager.Instance.EnemyHealthBuff(0.5f);
                    UIManager.Instance.ClearFlipStats();
                    UIManager.Instance.SetFlipStat("Enemy Speed", "200%", FlipStatsObject.ColourState.Negative);
                    UIManager.Instance.SetFlipStat("Enemy Damage", "66%", FlipStatsObject.ColourState.Positive);
                    UIManager.Instance.SetFlipStat("Enemy Health", "200%", FlipStatsObject.ColourState.Negative);
                    break;
                default:
                    WaveManager.Instance.EnemySpeedBuff(1f);
                    WaveManager.Instance.EnemyDamageBuff(1f);
                    WaveManager.Instance.EnemyHealthBuff(1f);
                    UIManager.Instance.ClearFlipStats();
                    UIManager.Instance.SetFlipStat("Enemy Speed", "100%", FlipStatsObject.ColourState.Neutral);
                    UIManager.Instance.SetFlipStat("Enemy Damage", "100%", FlipStatsObject.ColourState.Neutral);
                    UIManager.Instance.SetFlipStat("Enemy Health", "100%", FlipStatsObject.ColourState.Neutral);
                    break;
            }
        }
    }
}