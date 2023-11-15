using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

namespace INTENT
{
    [RequireComponent(typeof(Button))]
    public class ButtonControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        //[SerializeField] private UnityEvent onClick;
        [SerializeField] private UnityEvent onPointerEnter;
        [SerializeField] private UnityEvent onPointerExit;

        private Vector3 backupScale;

        public void OnPointerEnter(PointerEventData eventData)
        {
            onPointerEnter?.Invoke();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            onPointerExit?.Invoke();
        }

        public void OnEnable()
        {
            backupScale = transform.localScale;
        }
        public void OnDisable()
        {
            transform.localScale = backupScale;
        }

        public void ChangeScale(float scale)
        {
            backupScale = transform.localScale;
            transform.localScale = new Vector3(scale, scale, scale);
        }

        public void ChangeScale(float scaleX,float scaleY, float scaleZ)
        {
            backupScale = transform.localScale;
            transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }
    }
}
