using UnityEngine;

public class LevelController
{
    private LevelData _data = null;

    //LevelData内にある配列と突合させる
    private int _currentPhase = -1;

    private float _currentTimeLimit = 0f;
    private float _currentNormaPoint = 0f;

    public float CurrentTimeLimit { get => _currentTimeLimit; }
    public float CurrentNormaPoint { get => _currentNormaPoint; }

    public LevelController(LevelData levelData)
    {
        _data = levelData;
    }

    /// <summary>
    /// 最初のフェーズに初期化する
    /// </summary>
    public void SetStartPhase()
    {
        _currentPhase = 0;
        var phase = _data.GetPhaseData(_currentPhase);

        _currentTimeLimit = phase.TimeLimit;
        _currentNormaPoint = phase.NormaPoint;
    }

    /// <summary>
    /// 次のフェーズに移行する
    /// </summary>
    public void SetNextPhase()
    {
        ++_currentPhase;
        var phase = _data.GetPhaseData(_currentPhase);

        if (phase == null)
        {
            Debug.LogWarning("PhaseDataがnullです");

            _currentTimeLimit = 99999;
            _currentNormaPoint = 99999;

            return;
        }

        _currentTimeLimit = phase.TimeLimit;
        _currentNormaPoint = phase.NormaPoint;
    }

    public bool IsFinalPhase() => _currentPhase >= _data.PhaseLength() - 1;
}
