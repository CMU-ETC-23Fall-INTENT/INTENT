using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace INTENT
{
    public class EmailWindowWelcomeTextController : MonoBehaviour
    {
        [SerializeField] private TMP_Text welcomeText;
        [SerializeField] private string welcomeTextsPrefix;
        [SerializeField] private string welcomeTextsSuffix;
        [SerializeField] private bool shouldShowPlayerName = true;

        public void Initialize()
        {
        }
        public void OnEnable()
        {
            welcomeText.text = welcomeTextsPrefix + (shouldShowPlayerName ? GameManager.Instance.PlayerName : "") + welcomeTextsSuffix;
        }
    }
}
