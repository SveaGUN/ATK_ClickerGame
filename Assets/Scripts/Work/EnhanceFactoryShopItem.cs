using AkaneTools;
using TMPro;
using UnityEngine;

public class EnhanceFactoryShopItem : BaseShopItem
{
    [SerializeField]
    private TextMeshProUGUI _discriptionText = null;

    [SerializeField]
    private EnhanceFactoryItemData _data = null;

    private int _itemCount = 0;
    private bool _isSingleItem = false;

    protected override void OnInit()
    {
        //購入数が1の時、制限に達する場合は一度しか買えないアイテムとみなす
        _isSingleItem = _data.IsHitBuyLimit(1);

        _nameText.SetText(_data.ItemName);
        _discriptionText.SetText(_data.Discription);

        _costText.SetText(_data.ClacCost(0).ToString());
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
        AudioManager.Instance.PlaySE("BuyItem");
        ++_itemCount;

        gameData.CalcAddFactoryRate(_data.OutputEnhanceRate);

        GameManager.Instance.UpdatePPUIText();
        _costText.SetText(_data.ClacCost(_itemCount).ToString());

        if (_data.IsHitBuyLimit(_itemCount)) { _button.interactable = false; }
    }
}
