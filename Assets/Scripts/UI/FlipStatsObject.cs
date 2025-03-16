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
                    titleText.color = Color.green;
                    valueText.color = Color.green;
                    break;
                case ColourState.Negative:
                    titleText.color = Color.red;
                    valueText.color = Color.red;
                    break;
                case ColourState.Neutral:
                    titleText.color = Color.white;
                    valueText.color = Color.white;
                    break;
            }
        }
    }
}
