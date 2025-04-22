using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.DeterministicRoulette.Sound
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Sounds")]
        [SerializeField] private AudioClip _rollSound;
        [SerializeField] private AudioClip _dropSound;
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private AudioClip _chipSound;

        // Privates
        private AudioSource _audioSource;

        public static SoundManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayRollSound()
        {
            _audioSource.volume = 0.20f;
            _audioSource.clip = _rollSound;
            _audioSource.Play();
        }

        public void PlayDropSound()
        {
            _audioSource.volume = 0.20f;
            _audioSource.clip = _dropSound;
            _audioSource.Play();
        }

        public void PlayClickSound()
        {
            _audioSource.volume = 1.0f;
            _audioSource.clip = _clickSound;
            _audioSource.Play();
        }

        public void PlayChipSound()
        {
            _audioSource.volume = .20f;
            _audioSource.clip = _chipSound;
            _audioSource.Play();
        }
    }
}
