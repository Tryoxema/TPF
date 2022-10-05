namespace TPF.Internal
{
    internal static class ArrayHelper
    {
        internal static T[,] CreateLargerCopy<T>(T[,] original, int lengthIncrease, int heightIncrease)
        {
            var length = original.GetLength(0);
            var height = original.GetLength(1);

            var copy = new T[length + lengthIncrease, height + heightIncrease];

            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    copy[x, y] = original[x, y];
                }
            }

            return copy;
        }
    }
}