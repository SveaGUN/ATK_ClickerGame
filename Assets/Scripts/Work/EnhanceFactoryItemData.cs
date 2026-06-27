using UnityEngine;

[CreateAssetMenu(fileName = "EnhanceFactoryItemData", menuName = "Scriptable Objects/EnhanceFactoryItemData")]
public class EnhanceFactoryItemData : ScriptableObject
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
    private float _outputEnhanceRate = 1;

    [SerializeField]
    private int _maxBuyCount = 3;

    public string ItemName { get => _itemName; }
    public string Discription { get => _discription; }
    public float OutputEnhanceRate { get => _outputEnhanceRate;}

    public uint ClacCost(int n)
    {
        //コスト成長率^個数 * 基礎コスト
        return (uint)(Mathf.Pow(_costGrowthRate, n) * _baseCost);
    }

    public bool IsHitBuyLimit(int currentBuyCount) => _maxBuyCount <= currentBuyCount;
}
