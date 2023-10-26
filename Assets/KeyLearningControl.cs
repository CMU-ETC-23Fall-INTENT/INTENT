using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace INTENT
{
    public class KeyLearningControl : MonoBehaviour
    {
        [SerializeField] private List<GameObject> keyLearningPoints;
        [SerializeField] private GameObject keyLearningNavBar;
        [SerializeField] private GameObject Dots;
        [SerializeField] private GameObject DotSampleInactive;
        [SerializeField] private GameObject DotSampleActive;
        [SerializeField] private GameObject NextButton;
        [SerializeField] private GameObject PrevButton;
        [SerializeField] private GameObject NextButtonDisabled;
        [SerializeField] private GameObject PrevButtonDisabled;

        [SerializeField] public float Interval = 40f;

        private int currentIndex = 0;

        private bool dotsInitialized = false;
        [SerializeField] private List<GameObject> allDots;

        public void Initialize()
        {
            for (int i = 0; i < keyLearningPoints.Count; i++)
            {
                Transform newTransform = DotSampleActive.transform;
                Vector3 posDelta = new Vector3(Interval * (i - (float)(keyLearningPoints.Count - 1) / 2f), 0, 0);
                newTransform.position += posDelta;

                GameObject dot = Instantiate(DotSampleActive, Dots.transform);
                dot.name = "Dot " + i;
                dot.transform.localPosition = posDelta;
                dot.SetActive(true);
                allDots.Add(dot);
            }
        }

        public void Awake()
        {
            if (!dotsInitialized)
            {
                Initialize();
                dotsInitialized = true;
            }
        }

        public void Activate(int index)
        {
            currentIndex = index;

            keyLearningNavBar.SetActive(true);
            Dots.SetActive(true);

            NextButton.SetActive(false);
            PrevButton.SetActive(false);
            NextButtonDisabled.SetActive(false);
            PrevButtonDisabled.SetActive(false);

            GameObject nextButton = index == keyLearningPoints.Count - 1 ? NextButtonDisabled : NextButton;
            GameObject prevButton = index == 0 ? PrevButtonDisabled : PrevButton;

            nextButton.SetActive(true);
            prevButton.SetActive(true);

            for (int i = 0; i < keyLearningPoints.Count; i++)
            {
                allDots[i].GetComponent<Image>().sprite = i == index ? DotSampleActive.GetComponent<Image>().sprite : DotSampleInactive.GetComponent<Image>().sprite;
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
