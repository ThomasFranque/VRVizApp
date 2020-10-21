using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "Audio/Profile")]
    public class AudioProfile : ScriptableObject
    {
        [SerializeField] AudioSourceInfo[] _audioSources = default;
        public AudioSourceInfo[] AudioSources => _audioSources;
    }
}