using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    public class ElevatorTransitionController : Singleton<ElevatorTransitionController>
    {
        [SerializeField] private AnimationClip clip;
        [SerializeField] private TMPro.TMP_Text subtitleLeft;
        [SerializeField] private TMPro.TMP_Text subtitleRight;
        [SerializeField] private GameObject elevatorDoor;

        [YarnCommand("EpisodeTransition")]
        public static IEnumerator EpisodeTransition(string subtitleLeft, string subtitleRight, float waitTimeBeforeAnimation)
        {
            ElevatorTransitionController.Instance.SetSubtitle(subtitleLeft, subtitleRight);
            yield return Instance.PlayAnimationCoroutine(waitTimeBeforeAnimation);
        }

        private IEnumerator PlayAnimationCoroutine(float waitTimeBeforeAnimation)
        {
            yield return new WaitForSeconds(waitTimeBeforeAnimation);
            GetComponent<Animation>().Play(clip.name);
            yield return new WaitForSeconds(clip.length);
        }

        public void SetSubtitle(string subtitleTextLeft, string subtitleTextRight)
        {
            subtitleLeft.text = subtitleTextLeft;
            subtitleRight.text = subtitleTextRight;
        }
    }
}
