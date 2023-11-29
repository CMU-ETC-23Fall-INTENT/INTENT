using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class BadgeSelfControl : MonoBehaviour
    {
        private void OnDisable()
        {
            this.gameObject.SetActive(false);
        }
    }
}
