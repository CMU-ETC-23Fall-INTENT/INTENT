using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    public class ActivateDeactivateObjects : MonoBehaviour
    {
        [SerializeField]
        MeshRenderer meshRenderer;
      


        // Start is called before the first frame update
        void Start()
        {
        
        }


        [YarnCommand("EnableObject")]
        public void EnableMeshRenderer()
        {
            meshRenderer.enabled = true;
        }


        [YarnCommand("DisableObject")]
        public void DisableMeshRenderer()
        {
            meshRenderer.enabled = false;
        }
    }
}
