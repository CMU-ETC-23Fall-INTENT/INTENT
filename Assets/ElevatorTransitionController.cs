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
        public static IEnumerator EpisodeTransition(string subtitleLeft, string subtitleRight, float waitTimeBeforeAnimation, Episode episode)
        {
            ElevatorTransitionController.Instance.SetSubtitle(subtitleLeft, subtitleRight);
            yield return Instance.PlayAnimationCoroutine(waitTimeBeforeAnimation, episode);
        }

        private IEnumerator PlayAnimationCoroutine(float waitTimeBeforeAnimation, Episode episode)
        {
            yield return new WaitForSeconds(waitTimeBeforeAnimation);
            TaskManager.Instance.ActivateEpisode(episode);
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
