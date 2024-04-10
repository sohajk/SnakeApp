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

        internal static void DrawSnake(int newPositionX, int newPositionY, Snake snake, int score)
        {
            // Draw snake's head
            Console.SetCursorPosition(newPositionX, newPositionY);
            Console.ForegroundColor = snake.Head.Schermkleur;
            Console.Write("■");
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
            for (int i = 0; i < snake.Body.Count(); i++)
            {
                var bodyPart = snake.Body[i];
                Console.SetCursorPosition(bodyPart.PositionX, bodyPart.PositionY);
                Console.ForegroundColor = bodyPart.Schermkleur;
                Console.Write("■");
                Console.ResetColor();

                // TODO move to program check head for border and check head for body
                //if (snake.Body[i].PositionX == snake.Head.PositionX && snake.Body[i].PositionY == snake.Head.PositionY)
                //{
                //    _gameover = 1;
                //}
            }

            // Clear last snake's body part if score was raised
            if (snake.Body.Count > score)
            {
                Console.SetCursorPosition(snake.Body[0].PositionX, snake.Body[0].PositionY);
                Console.Write(" ");
                snake.Body.RemoveAt(0);
            }
        }

        internal static void DrawMovement()
        {
        }
    }
}
