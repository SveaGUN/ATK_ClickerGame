using AkaneTools;
using AkaneUtility;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : UIMonoBehaviour
{
    [SerializeField]
    private Button _retryButton = null;
    [SerializeField]
    private Button _titleButton = null;

    public event Action OnClickRetryButton;
    public event Action OnClickTitleButton;

    protected override void OnInit()
    {
        _retryButton.onClick.AddListener(OnClickRetry);
        _titleButton.onClick.AddListener(OnClickTitle);


        SetActive(false);
    }

    public void SetActive(bool value) => gameObject.SetActive(value);

    private void SetButtonInteractable(bool value)
    {
        _retryButton.interactable = value;
        _titleButton.interactable = value;
    }

    public void Show()
    {
        rectTransform.anchoredPosition = new Vector2(0, Screen.height);
        SetActive(true);
        SetButtonInteractable(true);

        StartCoroutine(ShowAnim());
    }

    private IEnumerator ShowAnim()
    {
        float animTime = 1f;
        float currentTime = 0f;

        float startY = rectTransform.anchoredPosition.y, endY = 0;

        yield return null;

        while (currentTime <= animTime)
        {
            var pos = rectTransform.anchoredPosition;
            pos.y = Mathf.Lerp(startY, endY, EasingUtility.EaseOutQuart(currentTime));
            rectTransform.anchoredPosition = pos;

            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    public void Hide()
    {
        StartCoroutine(HideAnim());
    }

    private IEnumerator HideAnim()
    {
        float animTime = 1f;
        float currentTime = 0f;

        float startY = rectTransform.anchoredPosition.y, endY = Screen.height;

        yield return null;

        while (currentTime <= animTime)
        {
            var pos = rectTransform.anchoredPosition;
            pos.y = Mathf.Lerp(startY, endY, EasingUtility.EaseInQuart(currentTime));
            rectTransform.anchoredPosition = pos;

            currentTime += Time.deltaTime;
            yield return null;
        }

        SetButtonInteractable(false);
        SetActive(false);
    }

    private void OnClickRetry()
    {
        OnClickRetryButton?.Invoke();

        AudioManager.Instance.PlaySE("ClickPointButton");
    }

    private void OnClickTitle()
    {
        OnClickTitleButton?.Invoke();

        AudioManager.Instance.PlaySE("ClickPointButton");
    }

    private void OnDestroy()
    {
        _retryButton.onClick.RemoveAllListeners();
        _titleButton.onClick.RemoveAllListeners();
    }
}
