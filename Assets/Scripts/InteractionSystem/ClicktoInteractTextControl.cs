using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class ClicktoInteractTextControl : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {            
            TextFaceCamera();
        }
        private void TextFaceCamera()
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
