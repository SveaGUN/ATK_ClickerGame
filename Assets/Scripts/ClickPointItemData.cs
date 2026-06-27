using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "ClickPointItemData", menuName = "Scriptable Objects/ClickPointItemData")]
public class ClickPointItemData : ScriptableObject
{
    [SerializeField]
    private string _itemName = "foo";

    [SerializeField, Tooltip("全角は1行7文字,3行まで")]
    private string _discription = "baa";

    [SerializeField]
    private uint _cost = 10;

    [SerializeField]
    private float _addClickPoint = 0;
    [SerializeField]
    private float _multiplyClickPoint = 1;

    public string ItemName { get => _itemName; }
    public string Discription { get => _discription; }
    public uint Cost { get => _cost; }
    public float AddClickPoint { get => _addClickPoint; }
    public float MultiplyClickPoint { get => _multiplyClickPoint; }

    /// <summary>
    /// 乗算してから加算する
    /// </summary>
    /// <param name="currentClickPoint"></param>
    /// <returns>効果適用後のクリックポイント</returns>
    public float ClacClickPoint(float currentClickPoint) => currentClickPoint * MultiplyClickPoint + AddClickPoint;
}
