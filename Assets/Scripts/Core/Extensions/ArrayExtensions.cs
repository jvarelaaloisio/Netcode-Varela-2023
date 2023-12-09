namespace Core.Extensions
{
    public static class ArrayExtensions
    {
        public static bool TryGetValueNotNull<T>(this T[] array, int index, out T value)
        {
            if (array.Length > index)
            {
                value = array[index];
                return value != null;
            }

            value = default;
            return false;
        }
    }
}