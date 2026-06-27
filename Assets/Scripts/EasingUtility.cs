using System;

namespace AkaneUtility
{
    public class EasingUtility
    {
        public static float EaseOutQuart(float x) => 1 - MathF.Pow(1 - x, 4);
    }
}