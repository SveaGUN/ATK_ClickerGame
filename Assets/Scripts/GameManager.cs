using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameData GameData { get; private set; } = null;

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
            DontDestroyOnLoad(this.gameObject);
        }
        else { Destroy(this.gameObject); }

        Application.targetFrameRate = 60;
    }

    //=======================デバッグ用===================
    private float d_introtime = 0.5f;

    //=======================デバッグ用 end===================

    private void Start()
    {
        //GameData = new GameData(1, 100, 30);
        GameData = new GameData(1, 999999, 86400);
        _timeLeftDiplayer.SetText(GameData.TimeLeft);
        _normaDisplayer.SetText(GameData.NormaPoint);
        _totalPointDiplayer.SetText(GameData.Point);
        _pointPerSecondDiplayer.SetText(GameData.PointPerSecond);
    }

    private void Update()
    {
        switch (_currentState)
        {
            case GameState.Intro:

                d_introtime -= Time.deltaTime;

                if (d_introtime <= 0)
                {
                    Debug.Log("START!!!");
                    OnGameStart();
                    _currentState = GameState.Play;
                }

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
                    _currentState = GameState.Clear;
                    return;
                }


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
}
