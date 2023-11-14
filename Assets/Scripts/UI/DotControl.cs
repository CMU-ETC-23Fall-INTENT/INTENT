using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace INTENT
{
    [RequireComponent(typeof(Button))]
    public class DotControl : MonoBehaviour
    {
        public void Initialize(int index, Action<int> onClick)
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() => onClick(index));
        }
    }
}
