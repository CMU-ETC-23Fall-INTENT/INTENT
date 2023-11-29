#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
namespace INTENT
{
    public class PreBuildCheck : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildReport report)
        {
            GameObject myGameObject = GameObject.Find("ElevatorForBeginning"); 
            if (myGameObject == null || !myGameObject.activeInHierarchy)
            {
                throw new BuildFailedException("Build failed: ElevatorForBeginning is not active in the scene.");
            }
        }
    }
}
#endif
