using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Settings")]
        [Range(0, 1)] public float volume = 1f;

        public static SoundManager Instance;
        protected virtual void Awake() 
        {
            Instance = this;
        }

        public virtual void PlayClipAtPoint(AudioClip clip)
        {
            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
            }
            else Debug.LogError("Clip is Null");
        }
    }
}
