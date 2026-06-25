using System.Collections;
using UnityEngine;

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
    private PointPerSecDisplayer _pointPerSecondDiplayer = null;
    public void SetPPSText() => _pointPerSecondDiplayer.SetText(GameData.PointPerSecond);

    [SerializeField]
    private TimeLeftDiplayer _timeLeftDiplayer = null;
    [SerializeField]
    private PointDisplayer _normaDisplayer = null;

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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }

        Application.targetFrameRate = 60;
    }

    //=======================デバッグ用===================
    private float d_introtime = 0.5f;

    //=======================デバッグ用 end===================

    private void Start()
    {
        _levelController = new(_leveData);
        _levelController.SetStartPhase();

        GameData = new GameData(_levelController.CurrentNormaPoint, _levelController.CurrentTimeLimit);
        //GameData = new GameData(1, 999999, 86400);

        _timeLeftDiplayer.SetText(0);//イントロアニメーションで時間はセットするので、最初は0でok
        _normaDisplayer.SetText(GameData.NormaPoint);
        _totalPointDiplayer.SetText(GameData.Point);
        _pointPerSecondDiplayer.SetText(GameData.PointPerSecond);

        StartCoroutine(Intro());
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

                GameData.AddPointPerSecond(Time.deltaTime);

                _timeLeftDiplayer.SetText(GameData.TimeLeft);
                _totalPointDiplayer.SetText(GameData.Point);

                if (GameData.IsTimeUp())
                {
                    OnGamePassed();
                    _currentState = GameState.GameOver;
                    return;
                }

                if (GameData.IsNormaClear())
                {
                    OnGamePassed();

                    //最終ラウンドではClear
                    //それ以外はPreparePlay
                    if (_levelController.IsFinalPhase()) { _currentState = GameState.Clear; }
                    else { _currentState = GameState.PreparePlay; }

                    return;
                }


                break;
            case GameState.PreparePlay:
                break;
            case GameState.GameOver:
                Debug.Log("GameOver");
                break;
            case GameState.Clear:
                Debug.Log("Clear");
                break;
            case GameState.Pause:
                break;
            default:
                break;
        }
    }

    //ゲームが始まった時の処理
    private void OnGameStart()
    {
        //ポイントの加算を有効にする
        _pointButton.OnClickPointButton += GameData.AddPointOnClick;
    }

    private void OnGamePassed()
    {   //ポイントの加算を無効にする
        _pointButton.OnClickPointButton -= GameData.AddPointOnClick;

    }

    private void OnDestroy()
    {
        _pointButton.OnClickPointButton -= GameData.AddPointOnClick;
    }

    private IEnumerator Intro()
    {
        const float animTime = 3f;
        float addTimePerSecond = GameData.TimeLimit / animTime;

        const int soundPlayNum = 10;//animTimeのうち何回音を鳴らすか
        float soundPlayTime = animTime / soundPlayNum;//音を鳴らす間隔

        float countUpTimeLimit = 0f;
        float currentAnimTime = 0f;
        float soundTimer = 0f;

        while (animTime > currentAnimTime)
        {
            countUpTimeLimit += addTimePerSecond * Time.deltaTime;
            _timeLeftDiplayer.SetText(countUpTimeLimit);

            if(soundTimer >= soundPlayTime)
            {
                //todo ---seの再生呼び出し---
                Debug.Log("se");

                soundTimer = 0f;
            }

            currentAnimTime += Time.deltaTime;
            soundTimer += Time.deltaTime;
            yield return null;
        }

        _timeLeftDiplayer.SetText(GameData.TimeLimit);
        OnGameStart();
        _currentState = GameState.Play;
    }
}
