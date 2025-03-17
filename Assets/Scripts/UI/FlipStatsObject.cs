using TMPro;
using UnityEngine;

namespace UpsideDown.UI
{
    public class FlipStatsObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text valueText;
        public enum ColourState
        {
            Positive,
            Negative,
            Neutral
        }
        
        public void Setup(string title, string value, ColourState colourState)
        {
            titleText.text = title;
            valueText.text = value;
            switch (colourState)
            {
                case ColourState.Positive:
                    valueText.color = Color.green;
                    break;
                case ColourState.Negative:
                    valueText.color = Color.red;
                    break;
                case ColourState.Neutral:
                    valueText.color = Color.white;
                    break;
            }
        }
    }
}
