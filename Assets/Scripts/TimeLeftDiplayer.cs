public class TimeLeftDiplayer : TextDisplayer
{
    public void SetText(float timeLeft)
    {
        _text.SetText(timeLeft.ToString("#"));
    }
}
