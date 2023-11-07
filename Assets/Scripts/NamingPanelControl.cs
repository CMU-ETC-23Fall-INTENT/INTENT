using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class NamingPanelControl : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_InputField InputField;
        [SerializeField] private List<GameObject> GameObjectsToActiveOnStart;
        // Start is called before the first frame update
        void Awake()
        {
            GameManager.Instance.ToggleIsPlayerHavingTutorial(true);
            PostProcessingControl.Instance.ToggleFade(true);

            foreach (var gameObject in GameObjectsToActiveOnStart)
            {
                gameObject.SetActive(true);
            }
        }

        public void OnFinishButtonClicked()
        {
            GameManager.Instance.ToggleIsPlayerHavingTutorial(false);
            PostProcessingControl.Instance.ToggleFade(false);

            //Get the name
            Debug.Log(InputField.text);
            GameManager.Instance.PlayerName = InputField.text;

            LoggingManager.Instance.Log("ChangePlayerName", InputField.text);
            this.gameObject.SetActive(false);
        }
    }
}
