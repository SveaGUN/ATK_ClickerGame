using UnityEngine;

public class UIMonoBehaviour : MonoBehaviour
{
    protected RectTransform rectTransform = null;

    //RectTransformは必ず取得させるため、Initはoverrideできないようにしている。
    //なので、初期化時に行いたい処理はOnInitに書くこと

    public void Init()
    {
        rectTransform = GetComponent<RectTransform>();

        OnInit();
    }

    protected virtual void OnInit() { }
}
