using AkaneTools;
using AkaneUtility;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField]
    private Button _button = null;

    [SerializeField]
    private Image _transitionImage = null;

    private void Start()
    {
        AudioManager.Instance.PlayBGM("Title");

        _button.onClick.AddListener(OnClick);

        var col = _transitionImage.color;
        col.a = 0f;
        _transitionImage.color = col;
        _transitionImage.enabled = false;
    }

    private void OnClick()
    {
        AudioManager.Instance.PlaySE("ClickPointButton");

        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        _transitionImage.enabled = true;
        AudioManager.Instance.FadeOutBGM(0.5f);

        float animTime = 2.1f;
        float invAnimTime = 1/ animTime;
        float currentTime = 0f;

        yield return null;

        while (currentTime <= animTime)
        {
            float amount = currentTime * invAnimTime;

            float t = 1 - EasingUtility.EaseOutQuart(amount);

            var col = _transitionImage.color;
            col.a = amount;
            _transitionImage.color = col;

            currentTime += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("MainGame");
    }
}
