using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioProfile _audioProfile = default;
        private static AudioManager _instance;

        private Dictionary<string, Sound> _soundCollection;
        private Dictionary<string, AudioSource> _audioSourceCollection;

        private void Awake()
        {
            if (_instance != default)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            CreateCollections();
        }

        private void CreateCollections()
        {
            _soundCollection = new Dictionary<string, Sound>();
            _audioSourceCollection = new Dictionary<string, AudioSource>(_audioProfile.AudioSources.Length);

            foreach (AudioSourceInfo i in _audioProfile.AudioSources)
            {
                AudioSource source;
                TimescalePitchShift pitchShift;
                CreateNewAudioSource(i.Name, out source);
                pitchShift = source.gameObject.AddComponent<TimescalePitchShift>();
                source.outputAudioMixerGroup = i.MixerGroup;
                _audioSourceCollection.Add(i.Name, source);

                foreach (Sound s in i.Sounds)
                {
                    s.AudioSource = source;
                    s.TimescalePitchShift = pitchShift;
                    _soundCollection.Add(s.Name, s);
                }
            }
        }

        private void CreateNewAudioSource(string ofName, out AudioSource newSource)
        {
            GameObject newSrcObj = new GameObject(ofName);
            newSrcObj.transform.SetParent(transform);
            newSource = newSrcObj.AddComponent<AudioSource>();
        }

        private void InstancePlay(string soundName, Action finishPlayCallback = default)
        {
            Sound s;
            if (_soundCollection.TryGetValue(soundName, out s))
            {
                AudioClip playedClip = s.Play();
                if (finishPlayCallback != default)
                    StartCoroutine(CDoInSeconds(playedClip.length, finishPlayCallback));
            }
            else
                Debug.LogWarning("Sound \"" + soundName + "\" was not found.");
        }

        public void Play(string soundName) => InstancePlay(soundName, default);

        private IEnumerator CDoInSeconds(float seconds, Action a)
        {
            yield return new WaitForSecondsRealtime(seconds);
            a?.Invoke();
        }

        public static void Play(string soundName, Action finishPlayCallback = default) =>
            _instance.InstancePlay(soundName, finishPlayCallback);
    }
}