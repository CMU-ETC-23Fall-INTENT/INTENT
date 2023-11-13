using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class ElevatorController : MonoBehaviour
    {
        [SerializeField] private AnimationClip clip;
        [SerializeField] private TMPro.TMP_InputField InputField;
        [SerializeField] private TMPro.TMP_Text WarningTextField;
        [SerializeField] private TutorialsControl Tutorials;

        [SerializeField] private List<GameObject> GameObjectsToEnableWhenGameStarts;


        void Awake()
        {
            foreach (GameObject gameObject in GameObjectsToEnableWhenGameStarts)
            {
                gameObject.SetActive(true);
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.ToggleIsPlayerHavingTutorial(true);
            //PostProcessingControl.Instance.ToggleFade(true);
            StartCoroutine(PlayAnimation(1.0f));
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public IEnumerator PlayAnimation(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            GetComponent<Animation>().Play(clip.name);
        }

        public void OnFinishButtonClicked()
        {
            //Get the name
            Debug.Log(InputField.text);
            if (!CheckNameLegal(InputField.text))
            {
                WarningTextField.text = "Please enter a valid name (1-10 characters)";
                return;
            }

            GameManager.Instance.ToggleIsPlayerHavingTutorial(false);
            //PostProcessingControl.Instance.ToggleFade(false);
            GameManager.Instance.PlayerName = InputField.text;
            LoggingManager.Instance.Log("ChangePlayerName", InputField.text);
            Tutorials.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }

        private bool CheckNameLegal(string name)
        {
            bool isLegal = true;
            isLegal &= name.Length > 0;
            isLegal &= name.Length < 10;
            return isLegal;
        }
    }
}
