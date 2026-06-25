using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;


namespace AkaneTools
{
    //BGMとSE、音量を管理するクラス
    //BGMのフェードイン、フェードアウトができる
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [Header("AudioClips")]

        [SerializeField]
        private SerializableDictionary<string, AudioClip> _bgmClipsDict = new();
        [SerializeField]
        private SerializableDictionary<string, AudioClip> _seClipsDict = new();
        [SerializeField]
        private SerializableDictionary<string, AudioClip> _uiClipsDict = new();

        [Header("AudioSources\n上はBGM,中はSE,下はUI")]
        [SerializeField]
        [Tooltip("BGMを再生するAudioSource")]
        private AudioSource _bgmSource = null;

        [SerializeField]
        [Tooltip("SEを再生するAudioSource")]
        private AudioSource _seSource = null;

        [SerializeField]
        [Tooltip("SEを再生するAudioSource")]
        private AudioSource _uiSource = null;

        //現在フェード中かどうか
        private bool _isBgmFading = false;

        public bool IsFading { get => _isBgmFading; }

        [SerializeField]
        private AudioMixer _audiMixer = null;

        public VolumeSetter Volume { get; private set; } = null;

        //====================================================================
        //初期化処理
        //====================================================================

        private void Awake()
        {
            //シングルトン
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                //辞書の初期化
                _bgmClipsDict.TryCreateDictionary();
                _seClipsDict.TryCreateDictionary();
                _uiClipsDict.TryCreateDictionary();

                Volume = new();
                Volume.Init(_audiMixer);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        //====================================================================
        //BGM処理
        //====================================================================

        /// <summary>
        /// BGMを再生する。フェード中は再生できない
        /// </summary>
        /// <param name="bgmName">BGM名</param>
        public void PlayBGM(string bgmName)
        {
            if (!_bgmClipsDict.ContainsKey(bgmName))
            {
                Debug.LogError($"BGM {bgmName} は存在しません");
                return;
            }

            if (_isBgmFading)
            {
                Debug.LogError("BGMがフェード中です");
                return;
            }

            //BGMが再生中なら停止する
            if (_bgmSource.isPlaying) { _bgmSource.Stop(); }

            //BGMを再生する
            _bgmSource.clip = _bgmClipsDict.GetElement(bgmName);
            _bgmSource.Play();
        }

        public void PauseBGM()
        {
            _bgmSource.Pause();
        }

        /// <summary>
        /// BGMを停止する。フェード中は停止不可
        /// </summary>
        public void StopBGM()
        {
            if (_isBgmFading)
            {
                Debug.LogError("BGMがフェード中です");
                return;
            }

            _bgmSource.Stop();
        }

        /// <summary>
        /// BGMを強制停止する。フェード中でも停止可
        /// </summary>
        public void ForcedStopBGM()
        {
            if (_isBgmFading)
            {
                StopAllCoroutines(); //全てのコルーチンを停止
                _isBgmFading = false; //フェード中フラグを解除
            }

            _bgmSource.Stop();
            Debug.LogWarning("BGMを強制停止しました");
        }

        //====================================================================
        //BGMフェードイン・フェードアウト処理
        //====================================================================

        //1は最大音量、0は最小音量

        /// <summary>
        /// BGMをフェードインさせる
        /// </summary>
        /// <remarks>
        /// <para>フェードイン時間 : 1 / fadeInAmount</para>
        /// フェードインの総フレーム数 : 1 / (fadeInAmount / FPS)
        /// </remarks>
        /// <param name="fadeInAmount">値が大きいほど、早くフェードインする</param>
        /// <param name="bgmName">BGM名</param>
        /// <param name="callback">フェードイン後に実行する関数</param>
        public void FadeInBGM(float fadeInAmount, string bgmName = null, Action callback = null)
        {
            _bgmSource.volume = 0;

            if (!string.IsNullOrEmpty(bgmName)) { PlayBGM(bgmName); }

            //フェードイン開始
            _isBgmFading = true;
            StartCoroutine(OnFadeInBGM(fadeInAmount, callback));
        }

        //BGMをフェードインさせる
        private IEnumerator OnFadeInBGM(float fadeInAmount, Action callback)
        {
            Debug.Log("BGMフェードイン開始");

            //フェードイン
            while (_bgmSource.volume < 1)
            {
                _bgmSource.volume += fadeInAmount * Time.deltaTime;
                yield return null;
            }

            //音量を最大にする
            _bgmSource.volume = 1;

            //フェードイン終了
            _isBgmFading = false;
            callback?.Invoke();
        }

        /// <summary>
        /// BGMをフェードアウトさせる
        /// </summary>
        /// <remarks>
        /// <para>フェードアウト時間 : 1 / fadeOutAmount</para>
        /// フェードアウトの総フレーム数 : 1 / (fadeOutAmount / FPS)
        /// </remarks>
        /// <param name="fadeOutAmount">値が大きいほど、早くフェードアウトする</param>
        /// <param name="isStop">フェードアウト時に、BGMを停止するか</param>
        /// <param name="callback">フェードアウト後に実行する関数</param>
        public void FadeOutBGM(float fadeOutAmount, bool isStop = true, Action callback = null)
        {
            //フェードアウト開始
            _isBgmFading = true;
            StartCoroutine(OnFadeOutBGM(fadeOutAmount, isStop, callback));
        }

        //BGMをフェードアウトさせる
        private IEnumerator OnFadeOutBGM(float fadeOutAmount, bool isStop, Action callback)
        {
            //フェードアウト
            while (_bgmSource.volume > 0)
            {
                _bgmSource.volume -= fadeOutAmount * Time.deltaTime;
                yield return null;
            }

            _bgmSource.volume = 0;

            if (isStop) { _bgmSource.Stop(); }

            //フェードアウト終了
            _isBgmFading = false;
            Debug.Log("BGMフェードアウト終了");
            callback?.Invoke();
        }

        //====================================================================
        //SE処理
        //====================================================================

        /// <summary>
        /// SEを再生する
        /// </summary>
        /// <param name="seName">SE名</param>
        public void PlaySE(string seName)
        {
            if (!_seClipsDict.ContainsKey(seName))
            {
                Debug.LogError($"SE {seName} は存在しません");
                return;
            }

            //SEを再生する
            _seSource.PlayOneShot(_seClipsDict.GetElement(seName));
        }

        /// <summary>
        /// UIのSEを再生する
        /// </summary>
        /// <param name="seName"></param>
        public void PlayUI(string seName)
        {
            if (!_uiClipsDict.ContainsKey(seName))
            {
                Debug.LogError($"UI_SE {seName} は存在しません");
                return;
            }

            //SEを再生する
            _uiSource.PlayOneShot(_uiClipsDict.GetElement(seName));
        }
    }
}