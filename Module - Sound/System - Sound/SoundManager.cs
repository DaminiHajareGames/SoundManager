using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommanOfDamini;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace FP11
{
    public class SoundManager : MonoBehaviour
    {
        public Transform parentOfAllHirarchyObj;

        #region DECLARATION
        //<PUBLIC VAR....
        public static SoundManager instance;

        public AudioMixerGroup soundAudioMixer;
        public AudioMixerGroup musicAudioMixer;

        public enum SoundTypes {
            gameplayMusic, uiBGMusic, genralButtonSound, diceRolling, kill, lose, pawnMove, singleHome, won, turnSwitch, tokenSlide, countDownGameStart, countDown321, playerLeftSound, count
        };
        public List<SoundProperties> soundProperties;

        [System.Serializable]
        public class SoundProperties
        {
            public SoundTypes soundType;
            public List<DirectSoundProperty> directSoundProperties = new List<DirectSoundProperty>();
            public List<SoundClipProperty> soundClipProperties = new List<SoundClipProperty>();
            [HideInInspector] public bool isPlaying;
            //Is music or sound
            //
        }

        [System.Serializable]
        public class SoundClipProperty : CommanSoundProperty
        {
            public AudioClip soundClip;
        }

        [System.Serializable]
        public class DirectSoundProperty : CommanSoundProperty
        { 
            public AudioSource soundAudioSource;
        }

        public class CommanSoundProperty
        {
            [SerializeField]
            [Range(0, 1)]
            public float _volume = 1;
            [SerializeField]
            public float volume
            {
                get
                {
                    return _volume;
                }
                set
                {
                    _volume = value;
                    if(sound != null)
                    {
                        sound.volume = _volume;
                    }
                }
            }
            public Sound.SoundType soundType;
            public bool isInLoopMode;
            [HideInInspector] public Sound sound;
        }
        //>PUBLIC VAR....

        //<HIDE IN INSPECTOR VAR...
        //Commamn 
        [HideInInspector] public List<Coroutine> listOfStartedCoroutines;
        //>HIDE IN INSPECTOR VAR...

        //<PRIVATE VAR....
        //>PRIVATE VAR....

        //<EVENT DELEGATE....
        //>EVENT DELEGATE....


        public static float soundStatusInFloat
        {
            get
            {
                return soundStatusInBool ? 1 : 0;
            }
        }

        public static float musicStatusInFloat
        {
            get
            {
                return musicStatusInBool ? 1 : 0;
            }
        }

        public static bool soundStatusInBool
        {
            get
            {
                return PlayerPrefs.GetInt("isSoundOn", 1) == 1 ? true : false;
            }
            set
            {
                PlayerPrefs.SetInt("isSoundOn", value ? 1 : 0);
                SoundManager.instance.SoundOrMusicStatusChanged();
            }
        }

        public static bool musicStatusInBool
        {
            get
            {
                return PlayerPrefs.GetInt("isMusicOn", 1) == 1 ? true : false;
            }
            set
            {
                PlayerPrefs.SetInt("isMusicOn", value ? 1 : 0);
                SoundManager.instance.SoundOrMusicStatusChanged();
            }
        }
        #endregion

        #region FUNCTION CALLING

        //<Comman Funs................................
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance.gameObject);
                instance = this;
            }
            Sound.musicAudioMixer = musicAudioMixer;
            Sound.soundAudioMixer = soundAudioMixer;
            //CommanForAllGame.SafeDontDestroyOnLoad(gameObject);
            //ReArrangeListAccordingToType();
        }

        void Start()
        {
            //<VAR INIT...
            listOfStartedCoroutines = new List<Coroutine>();

            for (int i = 0; i < soundProperties.Count; i++)
            {
                for (int j = 0; j < soundProperties[i].directSoundProperties.Count; j++)
                {
                    if (soundProperties[i].directSoundProperties[j].soundAudioSource != null)
                    {
                        soundProperties[i].directSoundProperties[j].soundAudioSource.Stop();
                    }
                }
            }

            //>VAR INIT...

            //<FUN CALLING...
            //>FUN CALLLING...

            //<COROUTINE…
            //>COROUTINE…

            //<EVENT DELEGATE INIT...
            //>EVENT DELEGATE INIT...
        }

        void ResetValues()
        {
            //<VAR INIT...
            //>VAR INIT...

            //<FUN CALLING...
            //>FUN CALLLING...

            //<COROUTINE…
            //>COROUTINE…

            //<EVENT DELEGATE INIT...
            //>EVENT DELEGATE INIT...
        }

        void OnGameOver()
        {
            //<VAR INIT...
            //>VAR INIT...

            //<FUN CALLING...
            //>FUN CALLLING...

            //<COROUTINE…
            //>COROUTINE…

            //<EVENT DELEGATE INIT...
            //>EVENT DELEGATE INIT...
        }

        void OnDestroy()
        {
            //Un Subscribe all subscribers....
            for (int i = 0; i < soundProperties.Count; i++)
            {
                StopSound(i);
            }
        }

        void StopAllStartedCoroutines()
        {
            if (listOfStartedCoroutines == null)
            {
                Debug.LogError("list of coroutine is not assigned " + this);
                return;
            }
            for (int i = 0; i < listOfStartedCoroutines.Count; i++)
            {
                if (listOfStartedCoroutines[i] != null)
                {
                    StopCoroutine(listOfStartedCoroutines[i]);           
                }
                else                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
                {
                }
            }
        }
        //>Comman Funs..........................

        //<Game Specific Funs.........................       

        public void GenrateSoundForGenralButton()
        {
            Debug.Log("Genrate sound button");
            GenrateSound(SoundTypes.genralButtonSound);
        }

        public void GenrateSound(int index)
        {
            GenrateSound((SoundTypes)index);
        }

        public void GenrateSound(SoundTypes soundType, Transform parentTransform = null)
        {
            SoundProperties tempSoundProperty = soundProperties[(int)soundType];

            SoundClipProperty tempSoundClipProperty;

            for (int i = 0; i < tempSoundProperty.soundClipProperties.Count; i++)
            {
                tempSoundClipProperty = tempSoundProperty.soundClipProperties[i];
                tempSoundClipProperty.sound = Sound.PlaySoundWithType(tempSoundClipProperty.soundClip, tempSoundClipProperty.volume, tempSoundClipProperty.soundType, tempSoundClipProperty.isInLoopMode);
                tempSoundClipProperty.sound.transform.SetParent(parentTransform);
                tempSoundClipProperty.sound.transform.localPosition = Vector3.zero;
            }     

            DirectSoundProperty tempDirectSoundProperty;

            for (int i = 0; i < tempSoundProperty.directSoundProperties.Count; i++)
            {
                tempDirectSoundProperty = tempSoundProperty.directSoundProperties[i];
                tempDirectSoundProperty.sound = Sound.PlaySoundWithType(tempDirectSoundProperty.soundAudioSource.clip, tempDirectSoundProperty.volume, tempDirectSoundProperty.soundType, tempDirectSoundProperty.isInLoopMode, tempDirectSoundProperty.soundAudioSource);
                tempDirectSoundProperty.sound.transform.SetParent(parentTransform);
                tempDirectSoundProperty.sound.transform.localPosition = Vector3.zero;
            }
        }

        public float GetAudioLengthOfSound(SoundTypes passedSoundType)
        {
            float maxAudioLength = 0;
            float currentAudioLength;
            SoundProperties tempSoundProperty = soundProperties[(int)passedSoundType];

            for (int i = 0; i < tempSoundProperty.directSoundProperties.Count; i++)
            {
                currentAudioLength = tempSoundProperty.directSoundProperties[i].soundAudioSource.clip.length;
                if (currentAudioLength > maxAudioLength)
                {
                    maxAudioLength = currentAudioLength;
                }
            }

            for (int i = 0; i < tempSoundProperty.soundClipProperties.Count; i++)
            {
                currentAudioLength = tempSoundProperty.soundClipProperties[i].soundClip.length;
                if (currentAudioLength > maxAudioLength)
                {
                    maxAudioLength = currentAudioLength;
                }
            }

            return maxAudioLength;
        }

        /*public void GenrateSound(SoundTypes soundType)
        {
            if (soundProperties[(int)soundType].isDirectSoundResource)
            {
                if (!PlayerPrefManager.instance.soundStatusInBool)
                {
                    for (int i = 0; i < soundProperties[(int)soundType].directSoundProperties.Count; i++)
                    {
                        soundProperties[(int)soundType].directSoundProperties[i].soundAudioSource.enabled = false;
                    }
                }
                else
                {
                    for (int i = 0; i < soundProperties[(int)soundType].directSoundProperties.Count; i++)
                    {
                        soundProperties[(int)soundType].directSoundProperties[i].soundAudioSource.enabled = true;
                        soundProperties[(int)soundType].directSoundProperties[i].soundAudioSource.volume = soundProperties[(int)soundType].directSoundProperties[i].volume.MappingValue(0, 1, 0, PlayerPrefManager.instance.soundStatusInFloat);
                        soundProperties[(int)soundType].directSoundProperties[i].soundAudioSource.Play();
                    }
                }
                soundProperties[(int)soundType].isPlaying = true;
            }
            else
            {
                for (int i = 0; i < soundProperties[(int)soundType].soundClipProperties.Count; i++)
                {
                    Sound.PlayOneShot(soundProperties[(int)soundType].soundClipProperties[i].soundClip);
                }
            }
        }*/

        public void StopSound(int index)
        {
            StopSound((SoundTypes)index);
        }

        public void StopSound(SoundTypes soundType)
        {
            for (int i = 0; i < soundProperties[(int)soundType].directSoundProperties.Count; i++)
            {
                if (soundProperties[(int)soundType].directSoundProperties[i].sound != null)
                {
                    soundProperties[(int)soundType].directSoundProperties[i].sound.StopSound();
                }
            }

            for (int i = 0; i < soundProperties[(int)soundType].soundClipProperties.Count; i++)
            {
                if (soundProperties[(int)soundType].soundClipProperties[i].sound != null)
                {
                    soundProperties[(int)soundType].soundClipProperties[i].sound.StopSound();
                }
            }
        }

        public void SoundOrMusicStatusChanged()
        {
            for (int i = 0; i < soundProperties.Count; i++)
            {
                for (int j = 0; j < soundProperties[i].directSoundProperties.Count; j++)
                {
                    if (soundProperties[i].directSoundProperties[j].sound != null)
                    {
                        soundProperties[i].directSoundProperties[j].sound.SetVolume();
                    }
                }

                for (int j = 0; j < soundProperties[i].soundClipProperties.Count; j++)
                {
                    if (soundProperties[i].soundClipProperties[j].sound != null)
                    {
                        soundProperties[i].soundClipProperties[j].sound.SetVolume();
                    }
                }
            }
        }
        //>Game Specific Funs.........................
#endregion

        #region COROUTINE

        IEnumerator CoroutineFun()
        {
            WaitForSeconds seconds = new WaitForSeconds(0.1f);
            while (true)
            {
                yield return seconds;
            }
        }
        #endregion

        #region EditorFun


#if UNITY_EDITOR
        //[ContextMenu("GiveButtonSoundEffectToAllButton")]
        /* void GiveButtonSoundEffectToAllButton()
         {
             FindAllObjectsInChild(parentOfAllHirarchyObj);

             parentOfAllHirarchyObj = null;
         }


         void FindAllObjectsInChild(Transform parent)
         {
             GameObject obj;
             for (int i = 0; i < parent.childCount; i++)
             {
                 obj = parent.GetChild(i).gameObject;
                 if (obj.GetComponent<Button>() != null)
                 {
                     UnityEditor.Events.UnityEventTools.AddPersistentListener(obj.GetComponent<Button>().onClick, () => GenrateSoundForGenralButton());
                 }

                 if (obj.transform.childCount != 0)
                 {
                     FindAllObjectsInChild(obj.transform);
                 }
             }
         }*/
#endif
#if UNITY_EDITOR
        [ContextMenu("Rearrange List")]
#endif
        public void ReArrangeListAccordingToType()
        {
            List<SoundProperties> tempInventoryInfo = new List<SoundProperties>(soundProperties);
            for (int i = 0; i < tempInventoryInfo.Count; i++)
            {
                soundProperties[(int)tempInventoryInfo[i].soundType] = tempInventoryInfo[i];
            }
        }
        

#endregion
    }
}
