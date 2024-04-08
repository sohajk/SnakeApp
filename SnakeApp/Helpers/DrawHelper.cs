namespace SnakeApp.Helpers
{
    internal static class DrawHelper
    {
        internal static void DrawWindowBorder(int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
            }
            for (int i = 0; i < width; i++)
            {
                Console.SetCursorPosition(i, height - 1);
                Console.Write("■");
            }
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
            }
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(width - 1, i);
                Console.Write("■");
            }
        }

        internal static void DrawMovement()
        {
        }
    }
}
