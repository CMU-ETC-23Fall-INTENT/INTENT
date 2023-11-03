using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    public class PostProcessingControl : Singleton<PostProcessingControl>
    {
        [SerializeField] public GameObject Fade;

        public void ToggleFade(bool isFadeIn)
        {
            Fade.SetActive(isFadeIn);
        }
    }
}
