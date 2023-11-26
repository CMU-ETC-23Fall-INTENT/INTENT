using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace INTENT
{
    public class ActionDistribution : PlayerAction
    {
        [SerializeField] private GameObject globalVolume;
        private VolumeProfile volumeProfile;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private GameObject defaultImage;
        [SerializeField] private GameObject distributionImage;
        [SerializeField] private DraggableWork currentWork;
        [SerializeField] private GameObject doneImage;
        [SerializeField] private DraggableWork[] works;
        [SerializeField] private WorkSlot[] workSlots;
        private int workCount = 0;
        public void AddWorkCount()
        {
            workCount++;
        }
        public int GetWorkCount()
        {
            return workCount;
        }
        public void StartDragWork(DraggableWork task)
        {
            currentWork = task;
        }
        public DraggableWork GetCurrentWork()
        {
            return currentWork;
        }
        private void OnEnable()
        {
            
            GameManager.Instance.PlayerEnterAction();
            virtualCamera.Priority = 11;
            StartCoroutine(StartWhiteBoard(1f));
        }
        private void Awake() 
        {
            volumeProfile = globalVolume.GetComponent<Volume>().profile;
        }
        public override void ResetAction()
        {
            foreach(DraggableWork work in works)
            {
                work.ResetWork();
            }
            foreach(WorkSlot slot in workSlots)
            {
                slot.ResetSlot();
            }
            workCount = 0;
            doneImage.SetActive(false);
            defaultImage.SetActive(true);
            distributionImage.GetComponent<CanvasGroup>().alpha = 0f;

        }
        public override void PerformAction()
        {
            if(volumeProfile.TryGet(out Tonemapping tonemapping))
            {
                tonemapping.mode.value = TonemappingMode.Neutral;
            }
            GameManager.Instance.PlayerExitAction();
            virtualCamera.Priority = 9;
            SuccessFinishAction();
        }
        public void FinsihActionCallback()
        {
            StartCoroutine(WaitAndPerformAction(1f));
        }
        IEnumerator WaitAndPerformAction(float sec)
        {

            doneImage.SetActive(true);
            yield return new WaitForSeconds(sec);
            PerformAction();
        }
        IEnumerator StartWhiteBoard(float sec)
        {
            yield return new WaitForSeconds(1.5f);
            if(volumeProfile.TryGet(out Tonemapping tonemapping))
            {
                tonemapping.mode.value = TonemappingMode.None;
            }
            float timer = 0f;
            while(timer < sec)
            {
                timer += Time.deltaTime;
                defaultImage.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, timer / sec);
                yield return null;
            }
            defaultImage.SetActive(false);
            defaultImage.GetComponent<CanvasGroup>().alpha = 0f;
            timer = 0f;
            while(timer < sec)
            {
                timer += Time.deltaTime;
                distributionImage.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, timer / sec);
                yield return null;
            }
            distributionImage.GetComponent<CanvasGroup>().alpha = 1f;
        }

    }
}
