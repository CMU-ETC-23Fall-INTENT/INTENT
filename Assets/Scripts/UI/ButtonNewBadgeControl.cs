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
            GetComponent<Button>().onClick.AddListener(OnClick);
        }
        private void OnClick()
        {
            badge.SetActive(false);
        }
        public void ShowNewBadge()
        {
            badge.SetActive(true);
        }


    }
}
