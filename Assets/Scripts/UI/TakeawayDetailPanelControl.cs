using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class TakeawayDetailPanelControl : MonoBehaviour
    {
        [SerializeField] private GameObject takeawayPanel;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void EnterFromTakeawayPanel(int idx)
        {
            takeawayPanel.SetActive(false);

            this.gameObject.SetActive(true);

        }
    }
}
