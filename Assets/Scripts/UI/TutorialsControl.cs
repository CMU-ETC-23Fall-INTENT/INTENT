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

        private void Start()
        {
            Activate(); // activate the first tutorial on start
        }

        public void Activate()
        {
            if (Tutorials.Count > 0)
            {
                GameManager.Instance.ToggleBlur(true); //enable Blur
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
                Tutorials[_index].SetActive(true);
                _index++;
            }
            else // if the last tutorial is active, close the window
            {
                gameObject.SetActive(false);
                GameManager.Instance.ToggleBlur(false); //disable Blur
            }
        }

    }
}
