using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace INTENT
{
    public class NavBarControl : MonoBehaviour
    {
        [SerializeField] private GameObject Dots;
        [SerializeField] private GameObject DotSampleInactive;
        [SerializeField] private GameObject DotSampleActive;
        [SerializeField] private Button NextButton;
        [SerializeField] private Button PrevButton;
        [SerializeField] private GameObject NextButtonDisabled;
        [SerializeField] private GameObject PrevButtonDisabled;

        [SerializeField] public float Interval = 40f;

        private bool dotsInitialized = false;
        [SerializeField] private List<GameObject> allDots;

        public void Initialize(int pointNum, Action onNext, Action onPrev, Action<int> onDotPressed)
        {
            if (dotsInitialized && allDots.Count == pointNum) return;
            foreach (GameObject dot in allDots)
            {
                Destroy(dot);
            }
            allDots.Clear();

            for (int i = 0; i < pointNum; i++)
            {
                Transform newTransform = DotSampleActive.transform;
                Vector3 posDelta = new Vector3(Interval * (i - (float)(pointNum - 1) / 2f), 0, 0);
                newTransform.position += posDelta;

                GameObject dot = Instantiate(DotSampleActive, Dots.transform);
                dot.name = "Dot " + i;
                dot.transform.localPosition = posDelta;
                dot.SetActive(true);
                dot.GetComponent<DotControl>()?.Initialize(i, onDotPressed);
                allDots.Add(dot);
            }
            dotsInitialized = true;
            NextButton.onClick.AddListener(() => onNext());
            PrevButton.onClick.AddListener(() => onPrev());
        }

        public void UpdateNavBar(int index)
        {
            Dots.SetActive(true);

            NextButton.gameObject.SetActive(false);
            PrevButton.gameObject.SetActive(false);
            NextButtonDisabled.SetActive(false);
            PrevButtonDisabled.SetActive(false);

            GameObject nextButton = index == allDots.Count - 1 ? NextButtonDisabled : NextButton.gameObject;
            GameObject prevButton = index == 0 ? PrevButtonDisabled : PrevButton.gameObject;

            nextButton.SetActive(true);
            prevButton.SetActive(true);

            for (int i = 0; i < allDots.Count; i++)
            {
                allDots[i].GetComponent<Image>().sprite = i == index ? DotSampleActive.GetComponent<Image>().sprite : DotSampleInactive.GetComponent<Image>().sprite;
            }
        }
    }
}
