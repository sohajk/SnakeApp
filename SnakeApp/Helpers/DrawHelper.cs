using SnakeApp.Models;

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

                Console.SetCursorPosition(i, height - 1);
                Console.Write("■");
            }

            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");

                Console.SetCursorPosition(width - 1, i);
                Console.Write("■");
            }
        }

        internal static Pixel GetRandomPixel(int windowWidth, int windowHeight, ConsoleColor consoleColor)
        {
            Random randomnummer = new Random();

            var pixel = new Pixel()
            {
                PositionX = randomnummer.Next(1, windowWidth - 2),
                PositionY = randomnummer.Next(1, windowHeight - 2),
                Schermkleur = consoleColor
            };

            return pixel;
        }

        internal static void DrawPixel(Pixel pixel)
        {
            Console.SetCursorPosition(pixel.PositionX, pixel.PositionY);
        }

        internal static void DrawMovement()
        {
        }
    }
}
