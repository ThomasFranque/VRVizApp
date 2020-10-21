using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    [System.Serializable]
    public class AudioSourceInfo
    {
        [SerializeField] private string _name = default;
        [SerializeField] private AudioMixerGroup _mixerGroup = default;
        [SerializeField] private Sound[] _sounds = default;

        public string Name => _name;
        public AudioMixerGroup MixerGroup => _mixerGroup;
        public Sound[] Sounds => _sounds;
    }
}