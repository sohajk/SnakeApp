using SnakeApp.Models;

namespace SnakeApp.Helpers
{
    internal static class DrawHelper
    {
        internal static void DrawWindowBorder(int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                DrawPixel(i, 0, ConsoleColor.Cyan);
                DrawPixel(i, height - 1, ConsoleColor.Cyan);
            }

            for (int i = 0; i < height; i++)
            {
                DrawPixel(0, i, ConsoleColor.Cyan);
                DrawPixel(width - 1, i, ConsoleColor.Cyan);
            }
        }

        internal static (int positionX, int positionY) GetRandomPixelPosition(int windowWidth, int windowHeight)
        {
            Random randomnummer = new Random();

            var positionX = randomnummer.Next(1, windowWidth - 2);
            var positionY = randomnummer.Next(1, windowHeight - 2);

            return (positionX, positionY);
        }

        internal static void DrawFruit(Pixel pixel)
        {
            DrawPixel(pixel);
        }

        internal static void DrawSnake(int newPositionX, int newPositionY, Snake snake, int score)
        {
            // Draw snake's head
            DrawPixel(newPositionX, newPositionY, snake.Head.Color);
            Console.ResetColor();

            // Clear previous head's position, if there is no body to be overwrited by
            if (snake.Body.Count == 0)
            {
                Console.SetCursorPosition(snake.Head.PositionX, snake.Head.PositionY);
                Console.Write(" ");
            }

            snake.Head.PositionX = newPositionX;
            snake.Head.PositionY = newPositionY;

            // Draw snake's body
            for (int i = 0; i < snake.Body.Count; i++)
            {
                var bodyPart = snake.Body[i];
                DrawPixel(bodyPart);
                Console.ResetColor();
            }

            // Clear last snake's body part if score was not raised
            if (snake.Body.Count > score)
            {
                Console.SetCursorPosition(snake.Body[0].PositionX, snake.Body[0].PositionY);
                Console.Write(" ");
                snake.Body.RemoveAt(0);
            }
        }

        private static void DrawPixel(Pixel pixel)
        {
            DrawPixel(pixel.PositionX, pixel.PositionY, pixel.Color);
        }

        private static void DrawPixel(int positionX, int positionY, ConsoleColor color)
        {
            Console.SetCursorPosition(positionX, positionY);
            Console.ForegroundColor = color;
            Console.Write("■");
            Console.ResetColor();
        }
    }
}
