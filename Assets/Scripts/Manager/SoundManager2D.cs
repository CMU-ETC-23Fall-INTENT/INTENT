using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    public class SoundManager2D : Singleton<SoundManager2D>
    {
        private AudioSource bgmSource;
        private AudioSource sfxSource;
        [SerializeField] private SerializableDictionary<string, AudioClip> bgmClips;
        [SerializeField] private SerializableDictionary<string, AudioClip> sfxClips;
        private Coroutine bgmCoroutine;
        private Coroutine sfxCoroutine;
        private float fakeTimeStamp = 0;


        // Start is called before the first frame update
        void Awake()
        {
            bgmSource = transform.GetChild(0).GetComponent<AudioSource>();
            sfxSource = transform.GetChild(1).GetComponent<AudioSource>();
            bgmSource.loop = true;
            sfxSource.loop = false;
            bgmSource.playOnAwake = false;
            sfxSource.playOnAwake = false;
        }

        [YarnCommand("FadePlayBGM")]
        public void FadePlayBGM(string bgmName)
        {
            if(bgmClips.ContainsKey(bgmName))
            {
                fakeTimeStamp = bgmSource.time;
                if(bgmCoroutine != null)
                {
                    StopCoroutine(bgmCoroutine);
                }
                bgmCoroutine = StartCoroutine(FadeStartBGM(2f, bgmClips[bgmName]));
            }
            else
            {
                Debug.LogError("BGM " + bgmName + " not found!");
            }
            
        }

        [YarnCommand("PlaySFX")]
        public void PlaySFX(string sfxName)
        {
            if(sfxClips.ContainsKey(sfxName))
            {
                sfxSource.PlayOneShot(sfxClips[sfxName]);
            }
            else
            {
                Debug.LogError("SFX " + sfxName + " not found!");
            }
        }
        [YarnCommand("StopSFX")]
        public void StopSFX()
        {
            if(sfxCoroutine != null)
            {
                StopCoroutine(sfxCoroutine);
            }
            sfxCoroutine = StartCoroutine(FadeStopSFX(1));
        }

        [YarnCommand("StopBGM")]
        public void StopBGM()
        {
            if(bgmCoroutine != null)
            {
                StopCoroutine(bgmCoroutine);
            }
            bgmCoroutine = StartCoroutine(FadeOutBGM(1));
        }

        IEnumerator FadeStartBGM(float speed, AudioClip newBGMClip)
        {
            yield return StartCoroutine(AdjustBGMTo(speed, 0));

            bgmSource.Stop();
            bgmSource.clip = newBGMClip;
            bgmSource.time = Mathf.Min(fakeTimeStamp, newBGMClip.length);
            bgmSource.Play();

            
            yield return StartCoroutine(AdjustBGMTo(speed, 1));
        }
        IEnumerator FadeOutBGM(float speed)
        {
            float startValume = bgmSource.volume;
            yield return StartCoroutine(AdjustBGMTo(speed, 0));
            bgmSource.Stop();
            bgmSource.volume = startValume;
        }

        public void SetBGMOn(bool isOn)
        {
            if(bgmCoroutine != null)
            {
                StopCoroutine(bgmCoroutine);
            }
            bgmCoroutine = StartCoroutine(AdjustBGMTo(1.0f, isOn ? 1 : 0));
        }

        private IEnumerator AdjustBGMTo(float speed, float targetVolume)
        {
            while (bgmSource.volume != targetVolume)
            {
                bgmSource.volume = Mathf.MoveTowards(bgmSource.volume, targetVolume, speed * Time.deltaTime);
                yield return null;
            }
            bgmSource.volume = targetVolume;
        }

        public void SetSFXOn(bool isOn)
        {
            if(sfxCoroutine != null)
            {
                StopCoroutine(sfxCoroutine);
            }
            sfxCoroutine = StartCoroutine(AdjustSFXTo(1.0f, isOn ? 1 : 0));
        }
        private IEnumerator FadeStopSFX(float speed)
        {
            yield return StartCoroutine(AdjustSFXTo(speed, 0));
            sfxSource.Stop();
            yield return new WaitForSeconds(0.5f);
            sfxSource.Play();
            sfxSource.volume = 1;
        }

        private IEnumerator AdjustSFXTo(float speed, float targetVolume)
        {
            while (sfxSource.volume != targetVolume)
            {
                sfxSource.volume = Mathf.MoveTowards(sfxSource.volume, targetVolume, speed * Time.deltaTime);
                yield return null;
            }
            sfxSource.volume = targetVolume;
        }
    }
}
