using TMPro;
using UnityEngine;

[RequireComponent (typeof(TextMeshProUGUI))]
public abstract class TextDisplayer<T> : MonoBehaviour
{
    protected TextMeshProUGUI _text = null;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

        OnStart();
    }

    public virtual void OnStart() { }

    public virtual void SetText(T text) => _text.SetText(text.ToString());
}
