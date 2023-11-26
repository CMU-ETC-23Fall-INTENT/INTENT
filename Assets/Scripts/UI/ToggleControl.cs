using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace INTENT
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleControl : MonoBehaviour
    {
        [SerializeField] private GameObject _toggleOn;
        [SerializeField] private GameObject _toggleOff;
        // reference to the bool value that it should control
        [SerializeField] private string statusName;

        private void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(Toggle);
        }

        private void OnEnable()
        {
            Toggle(StatusManager.GetStatus<bool>(statusName));
        }

        void Toggle(bool isOn)
        {
            _toggleOff.SetActive(!isOn);
            _toggleOn.SetActive(isOn);
            StatusManager.SetStatus(statusName, isOn);
        }
    }
}
