using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class ElevatorController : Singleton<ElevatorController>
    {
        [SerializeField] private AnimationClip clip;
        [SerializeField] private TMPro.TMP_InputField InputField;
        [SerializeField] private TMPro.TMP_Text WarningTextField;
        [SerializeField] private TutorialsControl Tutorials;
        [SerializeField] private GameObject CheckInButtonActivated;
        [SerializeField] private GameObject CheckInButtonUnActivated;

        [SerializeField] private List<GameObject> GameObjectsToEnableWhenAwake;
        [SerializeField] private List<GameObject> GameObjectsToEnableWhenGameStarts;
        [SerializeField] private List<GameObject> GameObjectsToDisableWhenGameStarts;

        void Awake()
        {
            foreach (GameObject gameObject in GameObjectsToEnableWhenAwake)
            {
                gameObject.SetActive(true);
            }
        }

        public void GameStart()
        {
            if (SaveManager.Savestates.HasName)
            {
                GameManager.Instance.PlayerName = SaveManager.Savestates.PlayerName;
                Tutorials.gameObject.SetActive(true);
                gameObject.SetActive(false);
                return;
            }
            foreach (GameObject gameObject in GameObjectsToEnableWhenGameStarts)
            {
                gameObject.SetActive(true);
            }
            foreach (GameObject gameObject in GameObjectsToDisableWhenGameStarts)
            {
                gameObject.SetActive(false);
            }
            InputField.onValueChanged.AddListener(OnInputFieldChanged);

            GameManager.Instance.ToggleIsPlayerHavingTutorial(true);
            //PostProcessingControl.Instance.ToggleFade(true);
            StartCoroutine(PlayAnimation(1.0f));
            SoundManager2D.Instance.PlaySFX("ElevatorSound");
        }

        // Start is called before the first frame update
        void Start()
        {
            //GameStart();
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
            SoundManager2D.Instance.PlaySFX("CheckIn01");
            SoundManager2D.Instance.PlaySFX("CheckIn02");
            GameManager.Instance.ToggleIsPlayerHavingTutorial(false);
            //PostProcessingControl.Instance.ToggleFade(false);
            GameManager.Instance.PlayerName = InputField.text;
            LoggingManager.Log("ChangePlayerName", InputField.text);
            SaveManager.Savestates.HasName = true;
            SaveManager.Savestates.PlayerName = InputField.text;
            Tutorials.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            TaskManager.Instance.ActivateEpisode(TaskManager.Instance.GetCurrentEpisode());
        }

        private bool CheckNameLegal(string name, out string reason)
        {
            bool isLegal = true;
            isLegal &= name.Length > 0;
            isLegal &= name.Length <= 20;
            if(!isLegal)
            {
                reason = "Name length should be between 1 and 20.";
                return isLegal;
            }

            isLegal &= System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z]+$");
            if (!isLegal)
            {
                reason = "Name should only contain letters only.";
                return isLegal;
            }
            reason = "";
            return isLegal;
        }
        public void OnInputFieldChanged(string name)
        {
            string reason;
            bool isNameLegal = CheckNameLegal(name, out reason);
            WarningTextField.text = isNameLegal? "" : "Please enter a name using 1-20 characters with a-z and A-Z.";
            CheckInButtonActivated.SetActive(isNameLegal);
            CheckInButtonUnActivated.SetActive(!isNameLegal);
        }
    }
}
