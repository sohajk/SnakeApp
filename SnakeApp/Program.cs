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
        private static Pixel _snakeHead;

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

            _fruit = DrawHelper.GetRandomPixel(_configuration.WindowWidth, _configuration.WindowHeight, ConsoleColor.White);
            _snakeHead = DrawHelper.GetRandomPixel(_configuration.WindowWidth / 2, _configuration.WindowHeight / 2, ConsoleColor.Red);

            int score = 0;
            _gameover = 0;

            string movement = "RIGHT";
            List<int> xposlijf = new List<int>();
            List<int> yposlijf = new List<int>();
            DateTime tijd = DateTime.Now;
            DateTime tijd2 = DateTime.Now;
            string buttonpressed = "no";

            while (_gameover != 1)
            {
                Console.Clear();

                DrawHelper.DrawWindowBorder(_configuration.WindowWidth, _configuration.WindowHeight);

                if (_snakeHead.PositionX == _configuration.WindowWidth - 1 || _snakeHead.PositionX == 0 || _snakeHead.PositionY == _configuration.WindowHeight - 1 || _snakeHead.PositionY == 0)
                {
                    _gameover = 1;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                if (_fruit.PositionX == _snakeHead.PositionX && _fruit.PositionY == _snakeHead.PositionY)
                {
                    score++;
                    _fruit = DrawHelper.GetRandomPixel(_configuration.WindowWidth, _configuration.WindowHeight, ConsoleColor.White);
                }
                for (int i = 0; i < xposlijf.Count(); i++)
                {
                    Console.SetCursorPosition(xposlijf[i], yposlijf[i]);
                    Console.Write("■");
                    if (xposlijf[i] == _snakeHead.PositionX && yposlijf[i] == _snakeHead.PositionY)
                    {
                        _gameover = 1;
                    }
                }
                if (_gameover == 1)
                {
                    break;
                }
                Console.SetCursorPosition(_snakeHead.PositionX, _snakeHead.PositionY);
                Console.ForegroundColor = _snakeHead.Schermkleur;
                Console.Write("■");
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
                xposlijf.Add(_snakeHead.PositionX);
                yposlijf.Add(_snakeHead.PositionY);
                switch (movement)
                {
                    case "UP":
                        _snakeHead.PositionY--;
                        break;
                    case "DOWN":
                        _snakeHead.PositionY++;
                        break;
                    case "LEFT":
                        _snakeHead.PositionX--;
                        break;
                    case "RIGHT":
                        _snakeHead.PositionX++;
                        break;
                }
                if (xposlijf.Count() > score)
                {
                    xposlijf.RemoveAt(0);
                    yposlijf.RemoveAt(0);
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
