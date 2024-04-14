using Microsoft.Extensions.Configuration;
using SnakeApp.Helpers;
using SnakeApp.Models;

namespace SnakeApp
{
    internal class Program
    {
        private static Configuration _configuration;
        private static int _gameover;
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

            var newPositionX = _configuration.WindowWidth / 2;
            var newPositionY = _configuration.WindowHeight / 2;

            _fruit = DrawHelper.GetRandomPixel(_configuration.WindowWidth, _configuration.WindowHeight, ConsoleColor.Cyan);
            _snake = new Snake(newPositionX, newPositionY);

            int score = 0;
            _gameover = 0;

            string movement = "RIGHT";
            DateTime tijd = DateTime.Now;
            DateTime tijd2 = DateTime.Now;
            string buttonpressed = "no";

            DrawHelper.DrawWindowBorder(_configuration.WindowWidth, _configuration.WindowHeight);

            while (_gameover != 1)
            {
                // Check hit with border
                if (newPositionX == _configuration.WindowWidth - 1 || newPositionX == 0 || newPositionY == _configuration.WindowHeight - 1 || newPositionY == 0)
                {
                    _gameover = 1;
                }

                // Check hit with body
                if (_snake.Body.Any(b => b.PositionX == newPositionX && b.PositionY == newPositionY))
                {
                    _gameover = 1;
                }

                _snake.Body.Add(new Pixel() { PositionX = _snake.Head.PositionX, PositionY = _snake.Head.PositionY, Schermkleur = ConsoleColor.Green });
                
                if (_gameover == 1)
                {
                    break;
                }

                // Check if get fruit
                if (_fruit.PositionX == _snake.Head.PositionX && _fruit.PositionY == _snake.Head.PositionY)
                {
                    score++;

                    _fruit = DrawHelper.GetRandomPixel(_configuration.WindowWidth, _configuration.WindowHeight, ConsoleColor.White);
                }

                DrawHelper.DrawSnake(newPositionX, newPositionY, _snake, score);


                DrawHelper.DrawPixel(_fruit);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("■");

                tijd = DateTime.Now;
                buttonpressed = "no";
                while (true)
                {
                    tijd2 = DateTime.Now;
                    if (tijd2.Subtract(tijd).TotalMilliseconds > 500) { break; }
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo toets = Console.ReadKey(true);
                        //Console.WriteLine(toets.Key.ToString());
                        if (toets.Key.Equals(ConsoleKey.UpArrow) && movement != "DOWN" && buttonpressed == "no")
                        {
                            movement = "UP";
                            buttonpressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.DownArrow) && movement != "UP" && buttonpressed == "no")
                        {
                            movement = "DOWN";
                            buttonpressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.LeftArrow) && movement != "RIGHT" && buttonpressed == "no")
                        {
                            movement = "LEFT";
                            buttonpressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.RightArrow) && movement != "LEFT" && buttonpressed == "no")
                        {
                            movement = "RIGHT";
                            buttonpressed = "yes";
                        }
                    }
                }

                switch (movement)
                {
                    case "UP":
                        newPositionY--;
                        break;
                    case "DOWN":
                        newPositionY++;
                        break;
                    case "LEFT":
                        newPositionX--;
                        break;
                    case "RIGHT":
                        newPositionX++;
                        break;
                }
            }

            Console.SetCursorPosition(_configuration.WindowWidth / 5, _configuration.WindowHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(_configuration.WindowWidth / 5, _configuration.WindowHeight / 2 + 1);
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
            _gameover = 1;
            ShutDown();

            Environment.Exit(0);
        }

        private static void ShutDown()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Press any key to close the app.");
            Console.ReadLine();
        }
    }
}
