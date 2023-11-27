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

        private float bgmStoredPauseTime;
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

        [YarnCommand("PlayBGM")]
        public void PlayBGM(string bgmName)
        {
            bgmStoredPauseTime = bgmSource.time;
            if(bgmClips.ContainsKey(bgmName))
            {
                bgmSource.clip = bgmClips[bgmName];
            }
            else
            {
                Debug.LogError("BGM " + bgmName + " not found!");
            }
            
        }

        [YarnCommand("FadePlayBGM")]
        public void FadePlayBGM(string bgmName)
        {
            bgmStoredPauseTime = bgmSource.time;
            if(bgmClips.ContainsKey(bgmName))
            {
                StartCoroutine(FadeStartBGM(0.5f, bgmClips[bgmName]));
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

        [YarnCommand("StopBGM")]
        public void StopBGM()
        {
            StartCoroutine(FadeOutBGM(0.5f));
        }

        IEnumerator FadeStartBGM(float sec, AudioClip newBGMClip)
        {
            float timer = 0;
            float startValume = bgmSource.volume;
            while(bgmSource.volume > 0)
            {
                timer += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(startValume, 0, timer/sec);
                yield return null;
            }

            timer = 0;
            bgmSource.Stop();
            bgmSource.clip = newBGMClip;
            bgmSource.time = bgmStoredPauseTime;
            bgmSource.Play();

            while(bgmSource.volume < 1)
            {
                timer += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(0, startValume, timer/sec);
                yield return null;
            }
        }
        IEnumerator FadeOutBGM(float sec)
        {
            float timer = 0;
            float startValume = bgmSource.volume;
            while(bgmSource.volume > 0)
            {
                timer += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(startValume, 0, timer/sec);
                yield return null;
            }

            timer = 0;
            bgmSource.Stop();
            bgmSource.volume = startValume;
        }

        public void SetBGMOn(bool isOn)
        {
            StartCoroutine(AdjustBGMTo(1.0f, isOn ? 1 : 0));
        }

        private IEnumerator AdjustBGMTo(float speed, float targetVolume)
        {
            while (bgmSource.volume != targetVolume)
            {
                bgmSource.volume = Mathf.MoveTowards(bgmSource.volume, targetVolume, speed * Time.deltaTime);
                yield return null;
            }
        }

        public void SetSFXOn(bool isOn)
        {
            StartCoroutine(AdjustSFXTo(1.0f, isOn ? 1 : 0));
        }

        private IEnumerator AdjustSFXTo(float speed, float targetVolume)
        {
            while (sfxSource.volume != targetVolume)
            {
                sfxSource.volume = Mathf.MoveTowards(sfxSource.volume, targetVolume, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
