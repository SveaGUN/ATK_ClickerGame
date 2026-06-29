using AkaneTools;
using AkaneUtility;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PreparePlayUI : UIMonoBehaviour
{
    [SerializeField]
    private Button _phaseStartButton = null;

    public event Action OnClickStartButton;

    [SerializeField]
    private PointDisplayer _pointDisplayer = null;
    [SerializeField]
    private PointDisplayer _clickPointDisplayer = null;
    [SerializeField]
    private PointPerSecDisplayer _ppsDisplayer = null;

    [SerializeField]
    private BonusUIAnim _bonusUIAnim = null;

    protected override void OnInit()
    {
        _phaseStartButton.onClick.AddListener(OnClick);
        _phaseStartButton.interactable = false;

        _bonusUIAnim.Init();

        SetActive(false);
    }

    public void SetActive(bool value) => gameObject.SetActive(value);

    public void Show()
    {
        rectTransform.anchoredPosition = new Vector2(0, Screen.height);
        SetActive(true);
        _phaseStartButton.interactable = true;

        UpdateTexts();

        StartCoroutine(ShowAnim());
    }

    private IEnumerator ShowAnim()
    {
        float animTime = 1f;
        float currentTime = 0f;

        float startY = rectTransform.anchoredPosition.y, endY = 0;

        yield return null;

        while(currentTime <= animTime)
        {
            var pos = rectTransform.anchoredPosition;
            pos.y = Mathf.Lerp(startY, endY, EasingUtility.EaseOutQuart(currentTime));
            rectTransform.anchoredPosition = pos;

            currentTime += Time.deltaTime;
            yield return null;
        }

        _bonusUIAnim.Show();
    }

    public void Hide()
    {
        StartCoroutine(HideAnim());
    }

    private IEnumerator HideAnim()
    {
        float animTime = 1f;
        float currentTime = 0f;

        float startY = rectTransform.anchoredPosition.y, endY = -(Screen.height * 2);

        yield return null;

        while (currentTime <= animTime)
        {
            var pos = rectTransform.anchoredPosition;
            pos.y = Mathf.Lerp(startY, endY, EasingUtility.EaseOutQuart(currentTime));
            rectTransform.anchoredPosition = pos;

            currentTime += Time.deltaTime;
            yield return null;
        }

        _phaseStartButton.interactable = false;

        _bonusUIAnim.Hide();
        SetActive(false);
    }

    private void OnClick()
    {
        OnClickStartButton?.Invoke();

        AudioManager.Instance.PlaySE("ClickPointButton");
    }

    public void UpdateTexts()
    {
        _pointDisplayer.SetText(GameManager.Instance.GameData.Point);
        _clickPointDisplayer.SetText(GameManager.Instance.GameData.ClickPoint);
        _ppsDisplayer.SetText(GameManager.Instance.GameData.FactoryPoint);
    }

    private void OnDestroy()
    {
        _phaseStartButton.onClick.RemoveAllListeners();
    }
}
