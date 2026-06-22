using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private AddPointItemData _data = null;

    private int _itemCount = 0;
    private float _currentTotalPointPerSecond = 0;

    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private TextMeshProUGUI _itemCountText = null;
    [SerializeField]
    private TextMeshProUGUI _ppsText = null;
    [SerializeField]
    private TextMeshProUGUI _costText = null;

    private Button _button = null;

    private void Awake()
    {
        _button = GetComponent<Button>();

        _nameText.SetText(_data.ItemName);
        _itemCountText.SetText(_itemCount.ToString());
        _ppsText.SetText(_data.PointPerSecond.ToString());
        _costText.SetText(_data.ClacCost(_itemCount).ToString());

        _button.onClick.AddListener(TryBuy);
    }

    private void TryBuy()
    {
        var gameData = GameManager.Instance.GameData;
        uint needCost = _data.ClacCost(_itemCount);

        if (needCost > gameData.Point)
        {
            Debug.Log("うわーん買えないよ～～～");
            return;
        }

        //購入分ポイントを消費する
        gameData.ClacSubtractPoint(needCost);
        ++_itemCount;

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

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
