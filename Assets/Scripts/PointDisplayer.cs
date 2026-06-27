public class PointDisplayer : TextDisplayer<float>
{
    public override void SetText(float point)
    {
        _text.SetText(point.ToString("#,0"));
    }
}
