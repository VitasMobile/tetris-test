using UnityEngine;

namespace TestTetris.Sounds
{
    public enum MusicType
    {
        Music1,
        Music2,
        Music3,
        Off
    }

    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        public AudioClip music1, music2, music3;

        private static MusicType _musicType;
        public static MusicType MusicType
        {
            get => _musicType;
            set
            {
                _musicType = value;
                PlayerPrefs.SetString("music", _musicType.ToString());
                Instance.UpdateState();
            }
        }

        private AudioSource _audioSource;


        #region SIMPLE SINGLETON
        private static MusicManager _instance;
        public static MusicManager Instance
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
            _audioSource = GetComponent<AudioSource>();

            _musicType = (MusicType)System.Enum.Parse(typeof(MusicType), PlayerPrefs.GetString("music", nameof(MusicType.Music1)));

            UpdateState();
        }

        private void UpdateState()
        {
            switch (_musicType)
            {
                case MusicType.Music1:
                    _audioSource.clip = music1;
                    break;

                case MusicType.Music2:
                    _audioSource.clip = music2;
                    break;

                case MusicType.Music3:
                    _audioSource.clip = music3;
                    break;

                default:
                case MusicType.Off:
                    _audioSource.Stop();
                    break;

            }

            if (_musicType != MusicType.Off)
            {
                _audioSource.Play();
            }
        }
    }
}
