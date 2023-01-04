using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helicoopter
{
    public class S_AudioManager : MonoBehaviour
    {
        public static S_AudioManager AudioManager;

        [SerializeField] private AudioClip[] audioClips;
        private void Awake()
        {
            if (AudioManager == null)
            {
                AudioManager = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetAudioClip(AudioSource source, AudioClips clip)
        {
            source.loop = false;
            source.clip = audioClips[(int) clip];
            source.Play();
        }

        public void SetAudioClipLoop(AudioSource source, AudioClips clip)
        {
            source.clip = audioClips[(int) clip];
            source.loop = true;
            source.Play();
        }
    }

    public enum AudioClips
    {
        UiJoin = 0,
        UiInteract,
        ShootRope,
        GetRope,
    }
}
