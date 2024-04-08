using Microsoft.Extensions.Configuration;
using SnakeApp.Helpers;
using SnakeApp.Models;

namespace SnakeApp
{
    internal class Program
    {
        private static Configuration _configuration;
        private static int _gameover;

        static void Main(string[] args)
        {
            var configBuilder = SetupConfiguration();

            if (configBuilder == null)
            {
                Console.WriteLine("The configuration file is missing.");
                ShutDown();

                Environment.Exit(0);
            }

            _configuration = configBuilder.GetSection(nameof(Configuration)).Get<Configuration>();

            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnCancelKeyPress);

            Random randomnummer = new Random();
            int score = 5;
            _gameover = 0;

            Pixel hoofd = new Pixel
            {
                PositionX = _configuration.WindowWidth / 2,
                PositionY = _configuration.WindowHeight / 2,
                Schermkleur = ConsoleColor.Red
            };

            string movement = "RIGHT";
            List<int> xposlijf = new List<int>();
            List<int> yposlijf = new List<int>();
            int berryx = randomnummer.Next(0, _configuration.WindowWidth);
            int berryy = randomnummer.Next(0, _configuration.WindowHeight);
            DateTime tijd = DateTime.Now;
            DateTime tijd2 = DateTime.Now;
            string buttonpressed = "no";

            while (_gameover != 1)
            {
                Console.Clear();
                if (hoofd.PositionX == _configuration.WindowWidth - 1 || hoofd.PositionX == 0 || hoofd.PositionY == _configuration.WindowHeight - 1 || hoofd.PositionY == 0)
                {
                    _gameover = 1;
                }

                DrawHelper.DrawWindowBorder(_configuration.WindowWidth, _configuration.WindowHeight);


                Console.ForegroundColor = ConsoleColor.Green;
                if (berryx == hoofd.PositionX && berryy == hoofd.PositionY)
                {
                    score++;
                    berryx = randomnummer.Next(1, _configuration.WindowWidth - 2);
                    berryy = randomnummer.Next(1, _configuration.WindowHeight - 2);
                }
                for (int i = 0; i < xposlijf.Count(); i++)
                {
                    Console.SetCursorPosition(xposlijf[i], yposlijf[i]);
                    Console.Write("■");
                    if (xposlijf[i] == hoofd.PositionX && yposlijf[i] == hoofd.PositionY)
                    {
                        _gameover = 1;
                    }
                }
                if (_gameover == 1)
                {
                    break;
                }
                Console.SetCursorPosition(hoofd.PositionX, hoofd.PositionY);
                Console.ForegroundColor = hoofd.Schermkleur;
                Console.Write("■");
                Console.SetCursorPosition(berryx, berryy);
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
                xposlijf.Add(hoofd.PositionX);
                yposlijf.Add(hoofd.PositionY);
                switch (movement)
                {
                    case "UP":
                        hoofd.PositionY--;
                        break;
                    case "DOWN":
                        hoofd.PositionY++;
                        break;
                    case "LEFT":
                        hoofd.PositionX--;
                        break;
                    case "RIGHT":
                        hoofd.PositionX++;
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
        private static IConfigurationRoot SetupConfiguration()
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
