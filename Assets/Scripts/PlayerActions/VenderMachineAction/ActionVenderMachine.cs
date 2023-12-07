using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

namespace INTENT
{
    public class ActionVenderMachine : PlayerAction
    {
        [SerializeField] private List<MachineSlot> slots = new List<MachineSlot>();
        [SerializeField] private Material lightMaterial;
        [SerializeField] private Material darkMaterial;
        [SerializeField] private GameObject lightCube;
        [SerializeField] private GameObject soldOutSign;
        [SerializeField] private CinemachineVirtualCamera highVendingMachineCamera;
        [SerializeField] private CinemachineVirtualCamera lowVendingMachineCamera;
        private int availableSlots;
        private int productWaitingToBePickedUp;

        private void Awake() 
        {
            soldOutSign.SetActive(false);
            foreach(Transform child in transform)
            {
                MachineSlot slot = child.GetComponent<MachineSlot>();
                if(slot != null)
                {
                    slots.Add(slot);
                }
            }
            availableSlots = slots.Count;
        }
        [ContextMenu("Start Action")]
        public override void StartAction()
        {
            if(availableSlots == 0)
            {
                GameManager.Instance.GetDialogueRunner().StartDialogue("R_NoMoreVending");
                GameManager.Instance.GetDialogueRunner().onDialogueComplete.AddListener(PerformAction);
                return;
            }
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("CharacterInvisibleInUI") | (1 << LayerMask.NameToLayer("InteractionPoints")));
            highVendingMachineCamera.Priority = 11;
            Camera.main.GetComponent<PhysicsRaycaster>().enabled = true;
            lightCube.GetComponent<MeshRenderer>().material = lightMaterial;
            GameManager.Instance.PlayerEnterAction();
            foreach(MachineSlot slot in slots)
            {
                slot.ActivateSlot();
            }
        }
        public override void ResetAction(int state)
        {
            foreach(MachineSlot slot in slots)
            {
                slot.ResetSlot();
            }
            soldOutSign.SetActive(false);
            availableSlots = slots.Count;
            productWaitingToBePickedUp = 0;
            Debug.Log("Reset Vender Machine");
        }
        public override void PerformAction()
        {
            if(availableSlots == 0)
            {
                soldOutSign.SetActive(true);
            }
            Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("CharacterInvisibleInUI") | (1 << LayerMask.NameToLayer("InteractionPoints"));
            highVendingMachineCamera.Priority = 9;
            lowVendingMachineCamera.Priority = 9;
            Camera.main.GetComponent<PhysicsRaycaster>().enabled = false;
            lightCube.GetComponent<MeshRenderer>().material = darkMaterial;
            GameManager.Instance.PlayerExitAction();
            SuccessFinishAction();
        }
        public void StopAllButtons(MachineSlot purchasedSlot, bool finished = false)
        {
            productWaitingToBePickedUp ++;
            StartCoroutine(PauseBeforeLookDown(1.5f));
            if(finished)
            {
                availableSlots --;
            }
            foreach(MachineSlot slot in slots)
            {
                if(slot != purchasedSlot)
                {
                    slot.DeactivateSlot();
                }
            }
        }
        public void ProductPickedUp()
        {
            productWaitingToBePickedUp --;
        }
        private IEnumerator PauseBeforeLookDown(float sec)
        {
            yield return new WaitForSeconds(sec);
            highVendingMachineCamera.Priority = 9;
            lowVendingMachineCamera.Priority = 11;
            yield return StartCoroutine(PauseBeforeFinish(5f));
        }
        private IEnumerator PauseBeforeFinish(float sec)
        {
            float timer = 0f;
            while (timer < sec && productWaitingToBePickedUp > 0)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            PerformAction();
        }
    }
}
