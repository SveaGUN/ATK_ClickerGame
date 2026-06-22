public class TotalPointDisplayer : TextDisplayer
{
    public void SetText(uint point)
    {
        _text.SetText(point.ToString("#,0"));
    }
}
