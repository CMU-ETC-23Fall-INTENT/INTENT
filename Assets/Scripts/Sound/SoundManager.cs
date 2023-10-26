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


        [Header("Riley")]
        [SerializeField]
        AudioSource rileyAudioSource;
        [SerializeField]
        AudioClip rileyKeyboardNormal, rileyKeyboardHard;



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



        [YarnCommand("RileyKeyboardNormal")]
        public void Riley_KeyboardTypingNormal()
        {
            rileyAudioSource.PlayOneShot(rileyKeyboardNormal);
        }

        [YarnCommand("RileyKeyboardHard")]
        public void Riley_KeyboardTypingHard()
        {            
            rileyAudioSource.PlayOneShot(rileyKeyboardHard);
        }
    }
}
