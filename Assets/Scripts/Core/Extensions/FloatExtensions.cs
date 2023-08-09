namespace Core
{
    public static class FloatExtensions
    {
        public static bool IsNegative(this float value) => value < 0;
        public static bool IsPositive(this float value) => value > 0;
    }
}
