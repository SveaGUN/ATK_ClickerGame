using UnityEngine;

[CreateAssetMenu(fileName = "AddPointItemData", menuName = "Scriptable Objects/AddPointItemData")]
public class AddPointItemData : ScriptableObject
{
    [SerializeField]
    private string _itemName = "foo";

    [SerializeField]
    private uint _baseCost = 10;

    [SerializeField]
    private float _costGrowthRate = 1.1f;

    [SerializeField]
    private float _pointPerSecond = 1;

    public string ItemName { get => _itemName; private set => _itemName = value; }
    public uint BaseCost { get => _baseCost; private set => _baseCost = value; }
    public float CostGrowthRate { get => _costGrowthRate; private set => _costGrowthRate = value; }
    public float PointPerSecond { get => _pointPerSecond; private set => _pointPerSecond = value; }

    public uint ClacCost(int n)
    {
        //コスト成長率^個数 * 基礎コスト
        return (uint)(Mathf.Pow(CostGrowthRate, n) * BaseCost);
    }
}
