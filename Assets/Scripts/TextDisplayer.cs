using TMPro;
using UnityEngine;

[RequireComponent (typeof(TextMeshProUGUI))]
public abstract class TextDisplayer : MonoBehaviour
{
    protected TextMeshProUGUI _text = null;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

        OnStart();
    }

    public virtual void OnStart() { }
}
