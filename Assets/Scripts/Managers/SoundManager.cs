using UnityEngine;

namespace TestTetris.Sounds
{
    public enum SoundClip
    {
        Turn,
        Move,
        Landed,
        RemoveLine,
        LevelUp,
        GameOver,
        TopScore,
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundClip sndClipName;
        public AudioClip sndClip;
        public bool isPlayOneShot = false;
        [HideInInspector] public AudioSource audioSource;
    }

    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        private AudioSource _mainAudioSource;
        public SoundAudioClip[] soundList;

        #region SIMPLE SINGLETON
        private static SoundManager _instance;
        public static SoundManager Instance
        {
            get {
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
        #endregion

        private void Start()
        {
            _mainAudioSource = GetComponent<AudioSource>();

            foreach (SoundAudioClip snd in soundList)
            {
                if (!snd.isPlayOneShot)
                {
                    GameObject soundGameObject = new GameObject($"Sound_{snd.sndClipName}");
                    soundGameObject.transform.SetParent(transform);
                    snd.audioSource = soundGameObject.AddComponent<AudioSource>();
                    snd.audioSource.clip = snd.sndClip;
                }
            }
        }
        public static void PlaySound(SoundClip sound)
        {
            SoundAudioClip snd = Instance.GetAudioClip(sound);
            if (snd.isPlayOneShot)
            {
                Instance._mainAudioSource.PlayOneShot(snd.sndClip);
            }
            else
            {
                snd?.audioSource.Play();
            }
        }

        private SoundAudioClip GetAudioClip(SoundClip sound)
        {
            foreach (SoundAudioClip snd in Instance.soundList)
            {
                if (snd.sndClipName == sound)
                {
                    return snd;
                }
            }

            Debug.LogError($"Sound {sound} not found!");
            return null;
        }
    }
}