using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    //Currently a band aid solution
    public class SoundManager : Singleton<SoundManager>
    {       
        [Header("Projector")]
        [SerializeField]
        AudioSource projectorAudioSource;
        [SerializeField]
        AudioClip projectorBroken, projectorOnAndOff, projectorFan;


        [Header("Ming")]
        [SerializeField]
        AudioSource mingAudioSource;
        [SerializeField]
        AudioClip mingKeyboardNormal, mingKeyboardHard;



        [YarnCommand("ProjectorBrokenAudio")]
        public void PlayAudio_ProjectorBroken()
        {
            projectorAudioSource.PlayOneShot(projectorBroken);
        }




        [YarnCommand("ProjectorOnAndOff")]
        public void PlayAudio_ProjectorOnAndOff()
        {
            projectorAudioSource.PlayOneShot(projectorOnAndOff);
        }


        [YarnCommand("ProjectorFanLoud")]
        public void PlayAudio_ProjectorFanLoud()
        {
            projectorAudioSource.PlayOneShot(projectorFan);
        }



        [YarnCommand("MingKeyboardNormal")]
        public void Ming_KeyboardTypingNormal()
        {
            mingAudioSource.PlayOneShot(mingKeyboardNormal);
        }

        [YarnCommand("MingKeyboardHard")]
        public void Ming_KeyboardTypingHard()
        {            
            mingAudioSource.PlayOneShot(mingKeyboardHard);
        }
    }
}
