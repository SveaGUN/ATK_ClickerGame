public class TimeLeftDiplayer : TextDisplayer<float>
{
    public override void SetText(float timeLeft)
    {
        _text.SetText(timeLeft.ToString("#"));
    }
}
