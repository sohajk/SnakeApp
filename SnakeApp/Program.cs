using Microsoft.Extensions.Configuration;
using SnakeApp.Models;

namespace SnakeApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configFilePath = Path.Combine(Environment.CurrentDirectory, "appsettings.json");
            if (!File.Exists(configFilePath))
            {
                Console.WriteLine("The configuration file is missing.");
                ShutDown();
                return;
            }

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var configuration = configBuilder.GetSection(nameof(Configuration)).Get<Configuration>();


            Random randomnummer = new Random();
            int score = 5;
            int gameover = 0;
            pixel hoofd = new pixel
            {
                xpos = configuration.WindowWidth / 2,
                ypos = configuration.WindowHeight / 2,
                schermkleur = ConsoleColor.Red
            };
            string movement = "RIGHT";
            List<int> xposlijf = new List<int>();
            List<int> yposlijf = new List<int>();
            int berryx = randomnummer.Next(0, configuration.WindowWidth);
            int berryy = randomnummer.Next(0, configuration.WindowHeight);
            DateTime tijd = DateTime.Now;
            DateTime tijd2 = DateTime.Now;
            string buttonpressed = "no";
            while (true)
            {
                Console.Clear();
                if (hoofd.xpos == configuration.WindowWidth - 1 || hoofd.xpos == 0 || hoofd.ypos == configuration.WindowHeight - 1 || hoofd.ypos == 0)
                {
                    gameover = 1;
                }
                for (int i = 0; i < configuration.WindowWidth; i++)
                {
                    Console.SetCursorPosition(i, 0);
                    Console.Write("■");
                }
                for (int i = 0; i < configuration.WindowWidth; i++)
                {
                    Console.SetCursorPosition(i, configuration.WindowHeight - 1);
                    Console.Write("■");
                }
                for (int i = 0; i < configuration.WindowHeight; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write("■");
                }
                for (int i = 0; i < configuration.WindowHeight; i++)
                {
                    Console.SetCursorPosition(configuration.WindowWidth - 1, i);
                    Console.Write("■");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                if (berryx == hoofd.xpos && berryy == hoofd.ypos)
                {
                    score++;
                    berryx = randomnummer.Next(1, configuration.WindowWidth - 2);
                    berryy = randomnummer.Next(1, configuration.WindowHeight - 2);
                }
                for (int i = 0; i < xposlijf.Count(); i++)
                {
                    Console.SetCursorPosition(xposlijf[i], yposlijf[i]);
                    Console.Write("■");
                    if (xposlijf[i] == hoofd.xpos && yposlijf[i] == hoofd.ypos)
                    {
                        gameover = 1;
                    }
                }
                if (gameover == 1)
                {
                    break;
                }
                Console.SetCursorPosition(hoofd.xpos, hoofd.ypos);
                Console.ForegroundColor = hoofd.schermkleur;
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
                xposlijf.Add(hoofd.xpos);
                yposlijf.Add(hoofd.ypos);
                switch (movement)
                {
                    case "UP":
                        hoofd.ypos--;
                        break;
                    case "DOWN":
                        hoofd.ypos++;
                        break;
                    case "LEFT":
                        hoofd.xpos--;
                        break;
                    case "RIGHT":
                        hoofd.xpos++;
                        break;
                }
                if (xposlijf.Count() > score)
                {
                    xposlijf.RemoveAt(0);
                    yposlijf.RemoveAt(0);
                }
            }
            Console.SetCursorPosition(configuration.WindowWidth / 5, configuration.WindowHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(configuration.WindowWidth / 5, configuration.WindowHeight / 2 + 1);
        }

        class pixel
        {
            public int xpos { get; set; }
            public int ypos { get; set; }
            public ConsoleColor schermkleur { get; set; }
        }

        private static void ShutDown()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to close the app.");
            Console.ReadLine();
        }
    }
}
