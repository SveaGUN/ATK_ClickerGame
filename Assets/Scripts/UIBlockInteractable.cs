using UnityEngine;
using UnityEngine.UI;

public class UIBlockInteractable : MonoBehaviour
{
    [SerializeField]
    private Image _pointBlock = null;

    [SerializeField]
    private Image _shopBlock = null;

    public void Init()
    {
        _shopBlock.enabled = true;
        _pointBlock.enabled = true;
    }

    public void SetActive(bool value)
    {
        _shopBlock.enabled = value;
        _pointBlock.enabled = value;
    }
}
