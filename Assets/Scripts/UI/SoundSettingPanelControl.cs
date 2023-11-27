using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class SoundSettingPanelControl : MonoBehaviour
    {
        private void OnEnable()
        {
            GameManager.ToggleBlurFromUI(true); //enable Blur
        }

        private void OnDisable()
        {
            GameManager.ToggleBlurFromUI(false); //disable Blur
        }
    }
}
