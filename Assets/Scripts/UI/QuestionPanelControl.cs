using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class QuestionPanelControl : MonoBehaviour
    {
        private void OnEnable()
        {
            GameManager.ToggleBlur(true); //enable Blur
        }

        private void OnDisable()
        {
            GameManager.ToggleBlur(false); //disable Blur
        }
    }
}
