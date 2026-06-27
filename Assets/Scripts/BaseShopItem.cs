using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseShopItem : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI _nameText = null;
    [SerializeField]
    protected TextMeshProUGUI _costText = null;

    protected Button _button = null;

    private void Awake()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(TryBuy);

        OnInit();
    }

    protected virtual void OnInit() { }
    protected virtual void OnDispose() { }

    protected abstract void TryBuy();

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();

        OnDispose();
    }
}
