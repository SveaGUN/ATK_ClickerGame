using AkaneTools;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PointButton : MonoBehaviour
{
    private Button _button = null;
    public event Action OnClickPointButton;

    private void Awake()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        OnClickPointButton?.Invoke();

        AudioManager.Instance.PlaySE("ClickPointButton");
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
