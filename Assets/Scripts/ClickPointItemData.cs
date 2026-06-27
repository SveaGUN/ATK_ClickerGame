using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "ClickPointItemData", menuName = "Scriptable Objects/ClickPointItemData")]
public class ClickPointItemData : ScriptableObject
{
    [SerializeField]
    private string _itemName = "foo";

    [SerializeField, Tooltip("全角は1行9文字,4行まで")]
    private string _discription = "baa";

    [SerializeField]
    private uint _baseCost = 10;
    [SerializeField]
    private float _costGrowthRate = 1.1f;

    [SerializeField]
    private float _addClickPoint = 0;
    [SerializeField]
    private float _multiplyClickPoint = 1;

    [SerializeField]
    private int _maxBuyCount = 3;

    public string ItemName { get => _itemName; }
    public string Discription { get => _discription; }

    public uint ClacCost(int n)
    {
        //コスト成長率^個数 * 基礎コスト
        return (uint)(Mathf.Pow(_costGrowthRate, n) * _baseCost);
    }

    /// <summary>
    /// 乗算してから加算する
    /// </summary>
    /// <param name="currentClickPoint"></param>
    /// <returns>効果適用後のクリックポイント</returns>
    public float ClacClickPoint(float currentClickPoint) => currentClickPoint * _multiplyClickPoint + _addClickPoint;

    public bool IsHitBuyLimit(int currentBuyCount) => _maxBuyCount <= currentBuyCount;
}
