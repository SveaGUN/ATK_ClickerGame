//現在のゲームの進行度を表す
using System;

[Serializable]
public class GameData
{
    public float Point { get; private set; } = 0f;
    public float PointPerSecond { get; private set; } = 0f;
    public float FactoryPoint { get; private set; } = 0f;
    public float ClickPoint { get; private set; } = 1f;
    public float OutputFactoryRate { get; private set; } = 1f;
    public float CurrentPhaseBonusPoint { get; private set; } = 0f;

    // n秒
    public float TimeLimit { get; private set; } = 0f;
    // 残り時間 カウントダウン方式
    public float TimeLeft { get; set; } = 0f;
    // n pt
    public float NormaPoint { get; set; } = 0f;
    public float TimeRatio { get =>  TimeLimit / TimeLeft; }

    /// <summary>
    /// ゲームデータを初期化する
    /// </summary>
    /// <param name="clickPoint">初期クリック値</param>
    /// <param name="normaPoint">ポイントのノルマ</param>
    /// <param name="timeLimit">制限時間(s)</param>
    public GameData(float normaPoint, float timeLimit, float clickPoint = 1)
    {
        ClickPoint = clickPoint;
        NormaPoint = normaPoint;
        TimeLimit = timeLimit;

        TimeLeft = TimeLimit;
    }

    public void SetNewNorma(float normaPoint, float timeLimit)
    {
        TimeLimit = timeLimit;
        NormaPoint = normaPoint;

        TimeLeft = TimeLimit;
    }

    /// <summary>
    /// 時間を進める
    /// </summary>
    /// <param name="deltaTime"></param>
    public void CountDown(float deltaTime) => TimeLeft -= deltaTime;

    public void SetClickPoint(float clickPoint) => ClickPoint = clickPoint;

    //====================ポイント計算関数====================
    public void ClacAddPoint(float point) => Point += point;
    public void ClacSubtractPoint(float point) => Point -= point;
    public void CalcAddPointPerSecond(float pps)
    {
        PointPerSecond += pps;
        FactoryPoint = PointPerSecond * OutputFactoryRate;
    }
    public void CalcAddFactoryRate(float rate)
    {
        OutputFactoryRate += rate - 1f;
        FactoryPoint = PointPerSecond * OutputFactoryRate;
    }
    public void CalcBonusPoint() => CurrentPhaseBonusPoint = TimeLeft / TimeLimit * 0.5f * NormaPoint;
    //======================================================

    //====================ポイント加算関数====================
    public void AddPointOnClick() => Point += ClickPoint;
    /// <summary>
    /// 毎秒得られるポイントを加算する
    /// 小数点以下はそのまま保持
    /// </summary>
    /// <param name="deltaTime"></param>
    public void AddFactoryPoint(float deltaTime) => Point += FactoryPoint * deltaTime;

    //=====================================================

    /// <summary>
    /// ノルマをクリアしたかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsNormaClear() => Point >= NormaPoint;

    public bool IsTimeUp() => TimeLeft <= 0f;
}
