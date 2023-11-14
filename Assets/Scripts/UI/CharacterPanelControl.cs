using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace INTENT
{
    public class CharacterPanelControl : MonoBehaviour
    {
        [SerializeField] private List<GameObject> characterPoints;
        [SerializeField] private NavBarControl characterNavBar;
        private int currentIndex = 0;

        public void Initialize()
        {
            characterNavBar.gameObject.SetActive(true);
            characterNavBar.Initialize(characterPoints.Count, Next, Prev, Activate);
            characterNavBar.gameObject.SetActive(false);
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

            characterNavBar.gameObject.SetActive(true);
            characterNavBar.UpdateNavBar(index);

            for (int i = 0; i < characterPoints.Count; i++)
            {
                characterPoints[i].SetActive(i == index);
            }
        }
        public void Next()
        {
            int nextIndex = currentIndex + 1;
            if (nextIndex < characterPoints.Count)
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
