using TMPro;
using UnityEngine;

namespace UNO.General
{
    public class LogObject : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI displaytext;

        public void SetText(string text)
        {
            displaytext.text = text;
        }

        public void SetColour(Color color)
        {
            displaytext.color = color;
        }
    }
}