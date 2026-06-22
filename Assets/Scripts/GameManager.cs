using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameData gameData = null;

    [SerializeField]
    private PointButton _pointButton = null;

    [SerializeField]
    private TotalPointDisplayer _totalPointDiplayer = null;
    [SerializeField]
    private TimeLeftDiplayer _timeLeftDiplayer = null;

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
    }

    //=======================デバッグ用===================
    private float d_introtime = 3f;

    //=======================デバッグ用 end===================

    private void Start()
    {
        gameData = new GameData(1, 100, 30);
        _timeLeftDiplayer.SetText(gameData.TimeLeft);
    }

    private void Update()
    {
        switch (_currentState)
        {
            case GameState.Intro:

                d_introtime -= Time.deltaTime;

                if(d_introtime <= 0)
                {
                    Debug.Log("START!!!");
                    OnGameStart();
                    _currentState = GameState.Play;
                }

                break;
            case GameState.Play:
                //時間を進める
                gameData.CountDown(Time.deltaTime);

                _timeLeftDiplayer.SetText(gameData.TimeLeft);
                _totalPointDiplayer.SetText(gameData.Point);

                if (gameData.IsTimeUp())
                {
                    OnGamePassed();
                    _currentState = GameState.GameOver;
                    return;
                }

                if (gameData.IsNormaClear())
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
        _pointButton.OnClickPointButton += gameData.AddPointOnClick;
    }

    private void OnGamePassed()
    {   //ポイントの加算を無効にする
        _pointButton.OnClickPointButton -= gameData.AddPointOnClick;

    }

    private void OnDestroy()
    {
        _pointButton.OnClickPointButton -= gameData.AddPointOnClick;
    }
}
