using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace INTENT
{
    public class RenderManager : MonoBehaviour
    {
        void Start()
        {
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
        }

        //https://docs.unity3d.com/ScriptReference/Rendering.RenderPipelineManager-beginCameraRendering.html
        void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            //String debugStr = String.Format("OnBeginCameraRendering {0}", camera.name);
            //Debug.Log(debugStr);

            camera.GetComponent<FocusCameraControl>()?.OnBeginCameraRendering(context);
        }

        void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            //String debugStr = String.Format("OnEndCameraRendering {0}", camera.name);
            //Debug.Log(debugStr);

            camera.GetComponent<FocusCameraControl>()?.OnEndCameraRendering(context);
        }

        void OnDestroy()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        }
    }
}
