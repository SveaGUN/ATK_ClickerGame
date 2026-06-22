public class PointPerSecDisplayer : TextDisplayer
{
    public void SetText(float point)
    {
        _text.SetText(point.ToString("0.0"));
    }
}
