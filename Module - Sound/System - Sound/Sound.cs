using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using CommanOfDamini;

namespace FP11
{
    public class Sound : MonoBehaviour
    {
        public static Sound music;

        public static GameObject source;

        public AudioSource audioSource;
        public static AudioMixerGroup soundAudioMixer;
        public static AudioMixerGroup musicAudioMixer;

        private bool _isThisSoundInstantiated = false;
        private float _volume;
        public float volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
                SetVolume();
            }
        }

        bool shouldDestroyAfterComplete = false;

        private SoundType _currentSoundType;
        public SoundType currentSoundType
        {
            get
            {
                return _currentSoundType;
            }
            set
            {
                _currentSoundType = value;
                switch(_currentSoundType)
                {
                    case SoundType.music:
                        audioSource.loop = true;
                        audioSource.outputAudioMixerGroup = musicAudioMixer;
                        break;

                    case SoundType.sound:
                        audioSource.outputAudioMixerGroup = soundAudioMixer;
                        break;
                }
            }
        }

        private AudioSource _audioComponent;
        public enum SoundType { sound, music };

        void Awake()
        {
            if (!source)
            {
                source = gameObject;
                gameObject.SetActive(false);

            }
            _audioComponent = GetComponent<AudioSource>();
        }

        public void SetVolume()
        {
            //Debug.Log("Set volume = " + PlayerPrefManager.instance.soundStatusInFloat + " music volume = " + PlayerPrefManager.instance.musicStatusInFloat);
            switch (currentSoundType)
            { 
                case SoundType.sound:
                    audioSource.volume = volume.MappingValue(0, 1, 0, SoundManager.soundStatusInFloat);
                    break;
                case SoundType.music:
                    audioSource.volume = volume.MappingValue(0, 1, 0, SoundManager.musicStatusInFloat);
                    break;
                default:
                    break;
            }
        }
            
        public void StopSound()
        {
            if (_isThisSoundInstantiated)
            {
                Destroy(gameObject);
            }
            else
            {
                audioSource.Stop();
            }
        }

        #region STATIC_METHODS
        public static Sound PlayOneShotSound(AudioClip clip, float passedVolume = 1, AudioSource passedAudioSource = null)
        {
            if (!SoundManager.soundStatusInBool) {
                return null;
            }
            return PlaySoundWithType(clip, passedVolume, SoundType.sound, false, passedAudioSource);
        }
        public static Sound PlayMusic(AudioClip clip, float passedVolume = 1, AudioSource passedAudioSource = null)
        {
            return PlaySoundWithType(clip, passedVolume, SoundType.music, true, passedAudioSource);
        }
        public static Sound PlaySoundInLoop(AudioClip clip, float passedVolume = 1, AudioSource passedAudioSource = null)
        {
            return PlaySoundWithType(clip, passedVolume, SoundType.sound, true, passedAudioSource);
        }

        public static Sound PlaySoundWithType(AudioClip clip, float passedVolume, SoundType passedSoundType, bool isInLoopMode, AudioSource passedAudioSource = null)
        {
            Sound spawnedSound;

            //Sound generate
            if (passedAudioSource != null)
            {
                if (passedAudioSource.GetComponent<Sound>() != null)
                {
                    spawnedSound = passedAudioSource.GetComponent<Sound>();
                }
                else
                {
                    spawnedSound = passedAudioSource.gameObject.AddComponent<Sound>();
                }
                spawnedSound.audioSource = passedAudioSource;
                spawnedSound.shouldDestroyAfterComplete = false;
                spawnedSound._isThisSoundInstantiated = false;
            }
            else
            {
                spawnedSound = Instantiate(source).GetComponent<Sound>();
                spawnedSound.shouldDestroyAfterComplete = true;
                spawnedSound._isThisSoundInstantiated = true;
            }

            spawnedSound.gameObject.SetActive(true);

            spawnedSound.currentSoundType = passedSoundType;
            spawnedSound.audioSource.loop = isInLoopMode;

            spawnedSound.volume = passedVolume;
            spawnedSound.audioSource.clip = clip;
            spawnedSound.name = clip.name;

            if (isInLoopMode)
            {
                spawnedSound.audioSource.Play();
            }
            else
            {
                spawnedSound.audioSource.PlayOneShot(clip);
            }

            return spawnedSound;
        }



        public static Sound PlayOneShot(AudioClip clip, float passedVolume = 1)
        {
            if (!SoundManager.soundStatusInBool)
            {
                return null;
            }

            var go = Instantiate(source) as GameObject;
            var s = go.GetComponent<Sound>();

            go.SetActive(true);
            s.audioSource.volume = passedVolume.MappingValue(0, 1, 0, SoundManager.soundStatusInFloat);

            s.audioSource.clip = clip;
            s.audioSource.PlayOneShot(clip);
            go.name = clip.name;

            return s;
        }

        public static Sound PlayInLoopAudio(AudioClip clip)
        {
            var go = Instantiate(source) as GameObject;
            var s = go.GetComponent<Sound>();

            go.SetActive(true);

            s.audioSource.loop = true;

            s.audioSource.clip = clip;

            s.audioSource.volume = SoundManager.musicStatusInFloat;

            if (SceneManager.GetActiveScene().name == "Game")
                s.audioSource.Play();

            music = s;

            return s;
        }

        public static void StopLoopAudio(Sound s)
        {
            s.audioSource.clip = null;
        }
        #endregion

        IEnumerator Start()
        {
            if (shouldDestroyAfterComplete && !audioSource.loop && (audioSource.clip != null))
            {
                yield return new WaitForSeconds(audioSource.clip.length + Random.value);
                Destroy(gameObject);
            }
        }

        public static void MuteMusic(bool state)
        {
            Debug.Log("Mute music = " + music);
            if (music != null)
                music.audioSource.volume = state ? 0 : 1;
        }
    }
}