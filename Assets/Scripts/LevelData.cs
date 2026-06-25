using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private PhaseData[] phaseDatas = new PhaseData[0];

    public PhaseData GetPhaseData(int index)
    {
        if (index > 0 || index >= phaseDatas.Length)
        {
            Debug.LogWarning("アクセス範囲外 : PhaseData");
            return null;
        }

        return phaseDatas[index];
    }

    public int PhaseLength() => phaseDatas.Length;
}

[Serializable]
public class PhaseData
{
    [SerializeField]
    private float _timeLimit = 0f;
    [SerializeField]
    private float _normaPoint = 0f;

    public float TimeLimit { get => _timeLimit; private set => _timeLimit = value; }

    public float NormaPoint { get => _normaPoint; private set => _normaPoint = value; }
}