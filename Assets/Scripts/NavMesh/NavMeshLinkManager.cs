using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace INTENT
{
    [System.Serializable]
    public class NavMeshLinkTransformPair
    {
        public Transform StartPoint;
        public Transform EndPoint;
        public bool Bidirectional = true;
    }
    public class NavMeshLinkManager : Singleton<NavMeshLinkManager>
    {
        [SerializeField] private List<NavMeshLinkTransformPair> NavMeshLinkTransformPairs;

        private void Awake()
        {
            foreach (var Pair in NavMeshLinkTransformPairs)
            {
                var navMeshLink = this.gameObject.AddComponent<NavMeshLink>();
                navMeshLink.startPoint = Pair.StartPoint.position;
                navMeshLink.endPoint = Pair.EndPoint.position;
                navMeshLink.bidirectional = Pair.Bidirectional;
            }
        }
        
    }
}
