using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace INTENT
{
    public class FocusCameraControl : MonoBehaviour
    {
        [SerializeField] private List<Light> lightToDisable;
        [SerializeField] private List<Light> lightToEnable;
        [SerializeField] private Transform modelTransform;
        private Dictionary<Light, bool> originalState = new Dictionary<Light, bool>();

        private Camera _camera;
        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void ChangeLayer(Transform transform, int layer)
        {
            transform.gameObject.layer = layer;
            foreach (Transform child in transform)
            {
                ChangeLayer(child, layer);
            }
        }
        public void OnBeginCameraRendering(ScriptableRenderContext context)
        {
            ChangeLayer(modelTransform, LayerMask.NameToLayer("Character"));

            foreach (Light light in lightToDisable)
            {
                originalState[light] = light.enabled;
                light.enabled = false;
            }
            foreach (Light light in lightToEnable)
            {
                originalState[light] = light.enabled;
                light.enabled = true;
            }
        }

        public void OnEndCameraRendering(ScriptableRenderContext context)
        {
            ChangeLayer(modelTransform, LayerMask.NameToLayer("CharacterInvisibleInUI"));

            foreach (Light light in lightToDisable)
            {
                light.enabled = originalState[light];
            }
            foreach (Light light in lightToEnable)
            {
                light.enabled = originalState[light];
            }
        }
    }
}
