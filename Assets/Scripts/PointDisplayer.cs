public class PointDisplayer : TextDisplayer
{
    public void SetText(float point)
    {
        _text.SetText(point.ToString("#,0"));
    }
}
