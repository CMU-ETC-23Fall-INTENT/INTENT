using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace INTENT
{
    public class TutorialsControl : MonoBehaviour
    {
        [SerializeField] public List<GameObject> Tutorials;
        private int _index = 0;
        private bool isActivated = false;

        private void Awake()
        {
            if (SaveManager.Savestates.DoneTutorial)
            {
                gameObject.SetActive(false);
                return;
            }
            Activate(); // activate the first tutorial on start
        }

        public void Activate()
        {
            LoggingManager.Log("Tutorial", "Activate");
            if(isActivated) return; // if the tutorial is already activated, do nothing (to prevent double activation)
            if (Tutorials.Count > 0)
            {
                isActivated = true;
                gameObject.SetActive(true);
                GameManager.ToggleBlur(true); //enable Blur
                GameManager.Instance.ToggleIsPlayerHavingTutorial(true);
                _index = 0;
                OnClick();
            }
        }

        public void OnClick()
        {
            foreach (var tutorial in Tutorials) // close all tutorials
            {
                tutorial.SetActive(false);
            }

            if(_index < Tutorials.Count) // if there are more tutorials, show the next one
            {
                LoggingManager.Log("Tutorial", Tutorials[_index].name);
                Tutorials[_index].SetActive(true);
                _index++;
            }
            else // if the last tutorial is active, close the window
            {
                LoggingManager.Log("Tutorial", "Finished");
                SaveManager.Savestates.DoneTutorial = true;
                isActivated = false;
                GameManager.ToggleBlur(false); //disable Blur
                GameManager.Instance.ToggleIsPlayerHavingTutorial(false);
                gameObject.SetActive(false);
            }
        }

    }
}
