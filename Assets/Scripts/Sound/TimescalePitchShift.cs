using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class TimescalePitchShift : MonoBehaviour
    {
        private const float SHIFT_SPEED = 1.5f;
        private AudioSource _audioSource;
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        // Update is called once per frame
        void Update()
        {
            _audioSource.pitch = Mathf.MoveTowards(_audioSource.pitch, Time.timeScale, Time.unscaledDeltaTime * SHIFT_SPEED);
        }
    }
}