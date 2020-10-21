using UnityEngine;

namespace Audio
{
    [System.Serializable]
    public class Sound
    {
        [SerializeField] private string _name = default;
        [Tooltip("Assigning more than one will activate random selection")]
        [SerializeField] private AudioClip[] _clips = default;
        [SerializeField, Range(0, 1)] private float _volume = 1;
        [SerializeField] private bool _isOneshot = true;
        [SerializeField] private bool _isAffectedByTimescale = false;
        [SerializeField] private bool _loop = false;

        public AudioClip[] Clips { get => _clips; set => _clips = value; }
        public float Volume { get => _volume; set => _volume = value; }
        public bool IsOneshot { get => _isOneshot; set => _isOneshot = value; }
        public bool Loop { get => _loop; set => _loop = value; }
        public string Name { get => _name; set => _name = value; }
        public bool IsAffectedByTimescale { get => _isAffectedByTimescale; set => _isAffectedByTimescale = value; }

        public AudioSource AudioSource { get; set; }
        public TimescalePitchShift TimescalePitchShift { get; set; }

        public AudioClip Play()
        {
            AudioClip clip;
            TimescalePitchShift.enabled = IsAffectedByTimescale;

            if (_clips.Length > 0)
                clip = _clips[Random.Range(0, _clips.Length)];
            else
                clip = _clips[0];

            if (IsOneshot)
            {
                AudioSource.PlayOneShot(clip, Volume);
            }
            else
            {
                AudioSource.Stop();
                AudioSource.volume = Volume;
                AudioSource.loop = Loop;
                AudioSource.clip = clip;
                AudioSource.Play();
            }
            return clip;
        }
    }
}