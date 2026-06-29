using AkaneTools;
using AkaneUtility;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameData GameData { get; private set; } = null;

    [SerializeField]
    private LevelData _leveData = null;
    private LevelController _levelController = null;

    [SerializeField]
    private PointButton _pointButton = null;

    [SerializeField]
    private PointDisplayer _totalPointDiplayer = null;
    [SerializeField]
    private PointPerSecDisplayer _factoryPointDiplayer = null;
    public void SetPPSText() => _factoryPointDiplayer.SetText(GameData.FactoryPoint);

    [SerializeField]
    private TimeLeftDiplayer _timeLeftDiplayer = null;
    [SerializeField]
    private PointDisplayer _normaDisplayer = null;
    [SerializeField]
    private IntDisplayer _phaseDisplayer = null;

    [SerializeField]
    private UIBlockInteractable _uiBlockInteractable = null;
    [SerializeField]
    private PreparePlayUI _preparePlayUI = null;
    public void UpdatePPUIText() => _preparePlayUI.UpdateTexts();

    [SerializeField]
    private GameOverUI _gameoverUI = null;
    [SerializeField]
    private GameClearUI _gameclearUI = null;

    private enum GameState
    {
        Intro,
        Play,
        PreparePlay,
        GameOver,
        Clear,
        Pause
    }

    private GameState _currentState = GameState.Intro;

    [SerializeField]
    private Image _transitionImage = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Application.targetFrameRate = 60;

        //繊維
        var col = _transitionImage.color;
        col.a = 0f;
        _transitionImage.color = col;
        _transitionImage.enabled = false;
    }

    //=======================デバッグ用===================


    //=======================デバッグ用 end===================

    private void Start()
    {
        //初期化
        _uiBlockInteractable.Init();
        _preparePlayUI.Init();
        _gameoverUI.Init();
        _gameclearUI.Init();

        _levelController = new(_leveData);
        _levelController.SetStartPhase();

        GameData = new(_levelController.CurrentNormaPoint, _levelController.CurrentTimeLimit);
        //GameData = new GameData(1, 999999, 86400);

        //テキストのセット
        _phaseDisplayer.SetText(_levelController.CurrentPhase);
        _timeLeftDiplayer.SetText(0);//イントロアニメーションで時間はセットするので、最初は0でok
        _normaDisplayer.SetText(GameData.NormaPoint);
        _totalPointDiplayer.SetText(GameData.Point);
        _factoryPointDiplayer.SetText(GameData.FactoryPoint);

        //イベント登録
        _pointButton.OnClickPointButton += GameData.AddPointOnClick;
        _preparePlayUI.OnClickStartButton += NextPhase;
        _gameoverUI.OnClickRetryButton += Retry;
        _gameclearUI.OnClickRetryButton += Retry;
        _gameoverUI.OnClickTitleButton += Title;
        _gameclearUI.OnClickTitleButton += Title;

        //イントロ開始
        StartCoroutine(PhaseStartIntro());
    }

    private void Update()
    {
        switch (_currentState)
        {
            case GameState.Intro:

                break;
            case GameState.Play:
                //時間を進める
                GameData.CountDown(Time.deltaTime);

                GameData.AddFactoryPoint(Time.deltaTime);

                _timeLeftDiplayer.SetText(GameData.TimeLeft);
                _totalPointDiplayer.SetText(GameData.Point);

                if (GameData.IsTimeUp())
                {
                    OnPhaseFailed();
                    return;
                }

                if (GameData.IsNormaClear())
                {
                    if (_levelController.IsFinalPhase())
                    {
                        OnLevelClear();
                    }
                    else
                    {
                        _currentState = GameState.PreparePlay;

                        OnPhasePassed();
                    }
                    return;
                }


                break;
            case GameState.PreparePlay:
                break;
            case GameState.GameOver:
                break;
            case GameState.Clear:
                break;
            case GameState.Pause:
                break;
            default:
                break;
        }
    }

    //ゲームが始まった時の処理
    private void OnPhaseStart()
    {
        if (!AudioManager.Instance.IsBGMPlaying) { AudioManager.Instance.PlayBGM("Main", 0.5f); }

        _uiBlockInteractable.SetActive(false);
    }

    private void OnPhasePassed()
    {
        //最終ラウンドではClear
        //それ以外はPreparePlay

        //ボーナスポイント
        //ノルマポイントのうち、残り時間の割合の半分を受け取る。最大で50%
        GameData.CalcBonusPoint();

        _uiBlockInteractable.SetActive(true);
        _preparePlayUI.Show();
    }

    private void OnPhaseFailed()
    {
        _currentState = GameState.GameOver;

        _uiBlockInteractable.SetActive(true);
        _gameoverUI.Show();
    }
    private void OnLevelClear()
    {
        _currentState = GameState.Clear;

        _uiBlockInteractable.SetActive(true);
        _gameclearUI.Show();
    }

    private void NextPhase()
    {
        _levelController.SetNextPhase();
        GameData.SetNewNorma(_levelController.CurrentNormaPoint, _levelController.CurrentTimeLimit);

        _phaseDisplayer.SetText(_levelController.CurrentPhase);
        _timeLeftDiplayer.SetText(0);//イントロアニメーションで時間はセットするので、最初は0でok
        _normaDisplayer.SetText(GameData.NormaPoint);
        _totalPointDiplayer.SetText(GameData.Point);
        _factoryPointDiplayer.SetText(GameData.FactoryPoint);

        _preparePlayUI.Hide();

        //イントロ開始
        StartCoroutine(PhaseStartIntro());
    }

    private void Retry()
    {
        _pointButton.OnClickPointButton -= GameData.AddPointOnClick;

        _levelController.SetStartPhase();
        GameData = new(_levelController.CurrentNormaPoint, _levelController.CurrentTimeLimit);

        _pointButton.OnClickPointButton += GameData.AddPointOnClick;

        _timeLeftDiplayer.SetText(0);//イントロアニメーションで時間はセットするので、最初は0でok
        _normaDisplayer.SetText(GameData.NormaPoint);
        _totalPointDiplayer.SetText(GameData.Point);
        _factoryPointDiplayer.SetText(GameData.FactoryPoint);

        if (_currentState == GameState.GameOver) { _gameoverUI.Hide(); }
        else { _gameclearUI.Hide(); }

        StartCoroutine(PhaseStartIntro(1f));
    }

    private void Title()
    {
        _pointButton.OnClickPointButton -= GameData.AddPointOnClick;
        StartCoroutine(LoadScene());
    }

    private void OnDestroy()
    {
        _preparePlayUI.OnClickStartButton -= NextPhase;
        _gameoverUI.OnClickRetryButton -= Retry;
        _gameclearUI.OnClickRetryButton -= Retry;
        _gameoverUI.OnClickTitleButton -= Title;
        _gameclearUI.OnClickTitleButton -= Title;
    }

    private IEnumerator PhaseStartIntro(float waitTime = 0f)
    {
        const float animTime = 2f;
        float addTimePerSecond = GameData.TimeLimit / animTime;

        int soundPlayNum = 20;//animTimeのうち何回音を鳴らすか
        float soundPlayTime = animTime / soundPlayNum;//音を鳴らす間隔

        float countUpTimeLimit = 0f;
        float currentAnimTime = 0f;
        float soundTimer = 0f;

        if (waitTime > 0) { yield return new WaitForSeconds(waitTime); }

        while (animTime > currentAnimTime)
        {
            countUpTimeLimit += addTimePerSecond * Time.deltaTime;
            _timeLeftDiplayer.SetText(countUpTimeLimit);

            if (soundTimer >= soundPlayTime)
            {
                AudioManager.Instance.PlaySE("CountUp");

                soundTimer = 0f;
            }

            currentAnimTime += Time.deltaTime;
            soundTimer += Time.deltaTime;
            yield return null;
        }

        //すぐに始めずちょっとだけ待機
        yield return new WaitForSeconds(0.5f);

        _timeLeftDiplayer.SetText(GameData.TimeLimit);
        OnPhaseStart();
        _currentState = GameState.Play;
    }

    private IEnumerator LoadScene()
    {
        _transitionImage.enabled = true;
        AudioManager.Instance.FadeOutBGM(0.5f);

        float animTime = 2.1f;
        float invAnimTime = 1 / animTime;
        float currentTime = 0f;

        yield return null;

        while (currentTime <= animTime)
        {
            float amount = currentTime * invAnimTime;

            float t = 1 - EasingUtility.EaseOutQuart(amount);

            var col = _transitionImage.color;
            col.a = amount;
            _transitionImage.color = col;

            currentTime += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("TitleScene");
    }
}
