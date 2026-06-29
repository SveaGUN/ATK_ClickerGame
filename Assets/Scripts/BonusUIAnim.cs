using AkaneUtility;
using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BonusUIAnim : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title = null;
    [SerializeField]
    private TextMeshProUGUI _point = null;
    [SerializeField]
    private TextMeshProUGUI _pt = null;

    private PointDisplayer _pointDisplayer = null;

    public void Init()
    {
        _point.enabled = false;

        if (_point.TryGetComponent<PointDisplayer>(out var disp)) { _pointDisplayer = disp; }
        else { Debug.LogWarning("PointDisplayer‚ª‚È‚¢‚¼"); }
    }

    public void SetActive(bool value) => gameObject.SetActive(value);
    public void TextSetActive(bool value)
    {
        _title.enabled = value;
        _point.enabled = value;
        _pt.enabled = value;
    }

    public void Show()
    {
        _pointDisplayer.SetText(GameManager.Instance.GameData.CurrentPhaseBonusPoint);

        StartCoroutine(ShowAnim());
    }

    private IEnumerator ShowAnim()
    {
        float animTime = 2f;
        float invAnimTime = 1f / animTime;
        float currentTime = 0f;

        float startSize = 150, endSize = 64;

        _point.enabled = true;
        _point.fontSize = startSize;

        var ratio = GameManager.Instance.GameData.TimeRatio;
        Debug.Log(ratio);
        if (ratio >= 0.9f) { _point.color = new Color(0.99f, 0.24f, 0.34f, 0f); }
        else if (ratio >= 0.7f) { _point.color = new Color(0f, 0.74f, 0.07f, 0f); }
        else if (ratio >= 0.5f) { _point.color = new Color(0.67f, 0.69f, 0.8f, 0f); }
        else if (ratio >= 0.3f) { _point.color = new Color(0.99f, 0.43f, 0.24f, 0f); }
        else if (ratio >= 0.1f) { _point.color = new Color(0f, 0f, 0f, 0f); }

        yield return null;

        while (currentTime <= animTime)
        {
            var t = currentTime * invAnimTime;
            var i = Mathf.Lerp(startSize, endSize, EasingUtility.EaseOutQuart(t));
            var a = Mathf.Lerp(0, 1, EasingUtility.EaseOutQuart(t));
            _point.fontSize = i;
            var col = _point.color;
            col.a =  a;
            _point.color = col;


            currentTime += Time.deltaTime;
            yield return null;
        }

        _point.fontSize = endSize;
    }

    public void Hide()
    {
        _point.enabled = false;
    }
}
