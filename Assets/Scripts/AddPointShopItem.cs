using AkaneTools;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddPointShopItem : BaseShopItem, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private AddPointItemData _data = null;

    private int _itemCount = 0;
    private float _currentTotalPointPerSecond = 0;

    [SerializeField]
    private TextMeshProUGUI _itemCountText = null;
    [SerializeField]
    private TextMeshProUGUI _ppsText = null;

    protected override void OnInit()
    {
        _nameText.SetText(_data.ItemName);
        _itemCountText.SetText(_itemCount.ToString());
        _ppsText.SetText(_data.PointPerSecond.ToString());
        _costText.SetText(_data.ClacCost(_itemCount).ToString());
    }

    protected override void TryBuy()
    {
        var gameData = GameManager.Instance.GameData;
        uint needCost = _data.ClacCost(_itemCount);

        if (needCost > gameData.Point)
        {
            AudioManager.Instance.PlaySE("CantBuyItem");
            return;
        }

        //購入分ポイントを消費する
        gameData.ClacSubtractPoint(needCost);
        ++_itemCount;
        AudioManager.Instance.PlaySE("BuyItem");

        _currentTotalPointPerSecond += _data.PointPerSecond;

        //1つずつ加算する
        //将来、バフなどで総ポイント数が必要なら_currentTotalPointPerSecondを使うこと
        gameData.CalcAddPointPerSecond(_data.PointPerSecond);

        //テキストの更新
        _itemCountText.SetText(_itemCount.ToString());
        _costText.SetText(_data.ClacCost(_itemCount).ToString());
        GameManager.Instance.SetPPSText();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
