using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class DialogueButtonControl : MonoBehaviour
    {
        [SerializeField] private GameObject GameObjectToSyncRectTransform;

        void Update()
        {
            if(GameObjectToSyncRectTransform != null)
            {
                CopyRectTransformSize(GameObjectToSyncRectTransform.GetComponent<RectTransform>(), GetComponent<RectTransform>());
            }
        }

        void CopyRectTransformSize(RectTransform copyFrom, RectTransform copyTo)
        {
            copyTo.offsetMin = copyFrom.offsetMin;
            copyTo.offsetMax = copyFrom.offsetMax;
            copyTo.anchorMin = copyFrom.anchorMin;
            copyTo.anchorMax = copyFrom.anchorMax;
            copyTo.anchoredPosition = copyFrom.anchoredPosition;
            copyTo.sizeDelta = copyFrom.sizeDelta;
        }
    }
}
