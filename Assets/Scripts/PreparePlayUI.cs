using AkaneTools;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PreparePlayUI : MonoBehaviour
{
    [SerializeField]
    private Button _phaseStartButton = null;

    public event Action OnClickStartButton;

    public void Init()
    {
        _phaseStartButton.onClick.AddListener(OnClick);
        SetActive(false);
    }

    public void SetActive(bool value) => gameObject.SetActive(value);

    private void OnClick()
    {
        OnClickStartButton?.Invoke();

        AudioManager.Instance.PlaySE("ClickPointButton");
    }

    private void OnDestroy()
    {
        _phaseStartButton.onClick.RemoveAllListeners();
    }
}
