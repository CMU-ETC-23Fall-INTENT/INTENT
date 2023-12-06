using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace INTENT
{
    [RequireComponent(typeof(Button))]
    public class ButtonNewBadgeControl : MonoBehaviour
    {
        [SerializeField] private GameObject badge;
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(HideNewBadge);
        }
        public void HideNewBadge()
        {
            SetNewBadgeActive(false);
        }
        public void ShowNewBadge()
        {
            SetNewBadgeActive(true);
        }
        public void SetNewBadgeActive(bool active)
        {
            badge.SetActive(active);
        }
        public bool IsNewBadgeActive()
        {
            return badge.activeSelf;
        }


    }
}
