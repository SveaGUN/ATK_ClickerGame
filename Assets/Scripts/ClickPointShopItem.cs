using AkaneTools;
using TMPro;
using UnityEngine;

public class ClickPointShopItem : BaseShopItem
{
    [SerializeField]
    private TextMeshProUGUI _discriptionText = null;

    [SerializeField]
    private ClickPointItemData _data = null;

    protected override void OnInit()
    {
        _nameText.SetText(_data.ItemName);
        _discriptionText.SetText(_data.Discription);
        _costText.SetText(_data.Cost.ToString());
    }

    protected override void TryBuy()
    {
        var gameData = GameManager.Instance.GameData;
        uint needCost = _data.Cost;

        if (needCost > gameData.Point)
        {
            AudioManager.Instance.PlaySE("CantBuyItem");
            return;
        }

        //w“ü•ªƒ|ƒCƒ“ƒg‚ğÁ”ï‚·‚é
        gameData.ClacSubtractPoint(needCost);
        AudioManager.Instance.PlaySE("BuyItem");

        var clickPoint = _data.ClacClickPoint(gameData.ClickPoint);
        gameData.SetClickPoint(clickPoint);

        GameManager.Instance.UpdatePPUIText();

        _button.interactable = false;
    }
}
