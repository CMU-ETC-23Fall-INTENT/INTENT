using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace INTENT
{
    public class KeyLearningControl : MonoBehaviour
    {
        [SerializeField] private List<GameObject> keyLearningPoints;
        [SerializeField] private NavBarControl keyLearningNavBar;

        private int currentIndex = 0;
        public void Initialize()
        {
            keyLearningNavBar.gameObject.SetActive(true);
            keyLearningNavBar.Initialize(keyLearningPoints.Count, Next, Prev, Activate);
            keyLearningNavBar.gameObject.SetActive(false);
        }

        public void Awake()
        {
            Initialize();
        }

        public void OnEnable()
        {
            Activate(0);
        }
        public void Activate(int index)
        {
            currentIndex = index;

            keyLearningNavBar.gameObject.SetActive(true);
            keyLearningNavBar.UpdateNavBar(index);

            for (int i = 0; i < keyLearningPoints.Count; i++)
            {
                keyLearningPoints[i].SetActive(i == index);
            }
        }
        public void Next()
        {
            int nextIndex = currentIndex + 1;
            if (nextIndex < keyLearningPoints.Count)
            {
                Activate(nextIndex);
            }
        }

        public void Prev()
        {
            int prevIndex = currentIndex - 1;
            if (prevIndex >= 0)
            {
                Activate(prevIndex);
            }
        }
    }
}
