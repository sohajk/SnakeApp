using Microsoft.Extensions.Configuration;
using SnakeApp.Helpers;
using SnakeApp.Models;

namespace SnakeApp
{
    internal class Program
    {
        private static Configuration _configuration;
        private static bool _gameover;
        private static int _score;
        private static Pixel _fruit;
        private static Snake _snake;

        static void Main(string[] args)
        {
            var configBuilder = GetConfiguration();

            if (configBuilder == null)
            {
                Console.WriteLine("The configuration file is missing.");
                ShutDown();

                Environment.Exit(0);
            }

            _configuration = configBuilder.GetSection(nameof(Configuration)).Get<Configuration>();

            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnCancelKeyPress);

            SetupConsoleWindow();

            _gameover = false;
            _score = 0;

            var (fruitPositionX, fruitPositionY) = DrawHelper.GetRandomPixelPosition(_configuration.WindowWidth, _configuration.WindowHeight);
            var (snakePositionX, snakePositionY) = DrawHelper.GetRandomPixelPosition(_configuration.WindowWidth - 10, _configuration.WindowHeight - 10);

            _fruit = new Pixel()
            {
                PositionX = fruitPositionX,
                PositionY = fruitPositionY,
                Color = ConsoleColor.Cyan
            };
            _snake = new Snake(snakePositionX, snakePositionY);

            DrawHelper.DrawWindowBorder(_configuration.WindowWidth, _configuration.WindowHeight);
            DrawHelper.DrawFruit(_fruit);

            RunGame();

            ShutDown();
        }

        private static void RunGame()
        {
            var newPositionX = _configuration.WindowWidth / 2;
            var newPositionY = _configuration.WindowHeight / 2;
            var movement = ConsoleKey.RightArrow;

            while (!_gameover)
            {
                // Check hit with border
                if (newPositionX == _configuration.WindowWidth - 1 || newPositionX == 0 || newPositionY == _configuration.WindowHeight - 1 || newPositionY == 0)
                {
                    _gameover = true;
                }

                // Check hit with body
                if (_snake.Body.Any(b => b.PositionX == newPositionX && b.PositionY == newPositionY))
                {
                    _gameover = true;
                }

                _snake.Body.Add(new Pixel() { PositionX = _snake.Head.PositionX, PositionY = _snake.Head.PositionY, Color = ConsoleColor.Green });

                if (_gameover == true)
                {
                    break;
                }

                // Check if get fruit
                if (_fruit.PositionX == _snake.Head.PositionX && _fruit.PositionY == _snake.Head.PositionY)
                {
                    _score++;

                    var (positionX, positionY) = DrawHelper.GetRandomPixelPosition(_configuration.WindowWidth, _configuration.WindowHeight);
                    _fruit.PositionX = positionX;
                    _fruit.PositionY = positionY;

                    DrawHelper.DrawFruit(_fruit);
                }

                DrawHelper.DrawSnake(newPositionX, newPositionY, _snake, _score);

                var timeStart = DateTime.Now;
                var timeStop = DateTime.Now;

                while (timeStop.Subtract(timeStart).TotalMilliseconds < _configuration.KeyPressedInterval)
                {
                    // Set movement only if a key has been already pressed, do not wait for it
                    if (Console.KeyAvailable)
                    {
                        movement = Console.ReadKey(true).Key;
                    }

                    timeStop = DateTime.Now;
                }

                switch (movement)
                {
                    case ConsoleKey.UpArrow:
                        newPositionY--;
                        break;
                    case ConsoleKey.DownArrow:
                        newPositionY++;
                        break;
                    case ConsoleKey.LeftArrow:
                        newPositionX--;
                        break;
                    case ConsoleKey.RightArrow:
                        newPositionX++;
                        break;
                }
            }
        }

        /// <summary>
        /// Try to set configuration from appsetings file.
        /// </summary>
        /// <returns>ConfigurationBuilder if the setup succeed, null otherwis.</returns>
        private static IConfigurationRoot GetConfiguration()
        {
            var configFilePath = Path.Combine(Environment.CurrentDirectory, "appsettings.json");
            if (!File.Exists(configFilePath))
            {
                return null;
            }

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            return configBuilder;
        }

        /// <summary>
        /// Set basic windows settings
        /// </summary>
        private static void SetupConsoleWindow()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(_configuration.WindowWidth, _configuration.WindowHeight);
        }

        /// <summary>
        /// Callback if ctrl+c is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _gameover = true;
            ShutDown();

            Environment.Exit(0);
        }

        private static void ShutDown()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Press any key to close the app.");

            Console.SetCursorPosition(_configuration.WindowWidth / 5, _configuration.WindowHeight / 2);
            Console.WriteLine("Game over, Score: " + _score);
            Console.SetCursorPosition(_configuration.WindowWidth / 5, _configuration.WindowHeight / 2 + 1);

            Console.ReadLine();
        }
    }
}
