using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class IndicatorSphereControl : MonoBehaviour
    {
        //[Range(0, 1)]
        //[SerializeField] private float alpha = 0.5f;

        [Range(1, 3)]
        [SerializeField] private float minScale = 2.4f;

        [SerializeField] private float maxScale = 3.0f;

        [SerializeField] private float loopTime = 1.5f;


        private float currentScale = 1.0f;
        // Start is called before the first frame update
        void Start()
        {
            currentScale = minScale;
            this.transform.localScale = new Vector3(minScale, minScale, minScale);
        }

        // Update is called once per frame
        void Update()
        {
            currentScale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(Time.time / loopTime, 1));
            this.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            //this.GetComponent<Renderer>().material.color = new Color(1, 1, 1, alpha);
        }
    }
}
