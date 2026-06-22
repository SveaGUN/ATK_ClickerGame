//現在のゲームの進行度を表す
public class GameData
{
    public uint Point { get; private set; } = 0;
    public uint GetPointPerSecond { get; set; } = 0;
    public uint ClickPoint { get; private set; } = 0;

    //n秒
    public float TimeLimit { get; private set; } = 0f;
    //残り時間 カウントダウン方式
    public float TimeLeft { get; set; } = 0f;
    //n pt
    public uint NormaPoint { get; set; } = 0;

    /// <summary>
    /// ゲームデータを初期化する
    /// </summary>
    /// <param name="clickPoint">初期クリック値</param>
    /// <param name="normaPoint">ポイントのノルマ</param>
    /// <param name="timeLimit">制限時間(s)</param>
    public GameData(uint clickPoint, uint normaPoint, float timeLimit)
    {
        ClickPoint = clickPoint;
        NormaPoint = normaPoint;
        TimeLimit = timeLimit;

        TimeLeft = TimeLimit;
    }

    /// <summary>
    /// 制限時間を設定する
    /// ゲームが新しく始まるときに呼び出す
    /// </summary>
    /// <param name="timeLimit"></param>
    public void SetTimeLimit(float timeLimit)
    {
        TimeLimit = timeLimit;

        TimeLeft = TimeLimit;
    }

    /// <summary>
    /// 時間を進める
    /// </summary>
    /// <param name="deltaTime"></param>
    public void CountDown(float deltaTime) => TimeLeft -= deltaTime;

    public void AddPoint(uint point) => Point += point;
    public void AddPointOnClick() => Point += ClickPoint;

    /// <summary>
    /// ノルマをクリアしたかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsNormaClear() => Point >= NormaPoint;

    public bool IsTimeUp() => TimeLeft <= 0;
}
