using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class ElevatorController : MonoBehaviour, ISaveable
    {
        [SerializeField] private AnimationClip clip;
        [SerializeField] private TMPro.TMP_InputField InputField;
        [SerializeField] private TMPro.TMP_Text WarningTextField;
        [SerializeField] private TutorialsControl Tutorials;
        [SerializeField] private GameObject CheckInButtonActivated;
        [SerializeField] private GameObject CheckInButtonUnActivated;

        [SerializeField] private List<GameObject> GameObjectsToEnableWhenGameStarts;


        void Awake()
        {
            foreach (GameObject gameObject in GameObjectsToEnableWhenGameStarts)
            {
                gameObject.SetActive(true);
            }
            InputField.onValueChanged.AddListener(OnInputFieldChanged);
            SaveManager.RegisterSaveable(this);
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

            GameManager.Instance.ToggleIsPlayerHavingTutorial(false);
            //PostProcessingControl.Instance.ToggleFade(false);
            GameManager.Instance.PlayerName = InputField.text;
            LoggingManager.Log("ChangePlayerName", InputField.text);
            Tutorials.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }

        private bool CheckNameLegal(string name, out string reason)
        {
            bool isLegal = true;
            isLegal &= name.Length > 0;
            isLegal &= name.Length <= 30;
            if(!isLegal)
                reason = "Name length should be between 1 and 30";
            else
                reason = "";
            return isLegal;
        }
        public void OnInputFieldChanged(string name)
        {
            string reason;
            bool isNameLegal = CheckNameLegal(name, out reason);
            WarningTextField.text = isNameLegal? "" : "Please enter your name (1-30 characters)";
            CheckInButtonActivated.SetActive(isNameLegal);
            CheckInButtonUnActivated.SetActive(!isNameLegal);
        }

        public Dictionary<string, string> GetSaveData()
        {
            throw new System.NotImplementedException();
        }

        public void SetSaveData(Dictionary<string, string> saveData)
        {
            throw new System.NotImplementedException();
        }
    }
}
