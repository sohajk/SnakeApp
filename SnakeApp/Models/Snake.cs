namespace SnakeApp.Models
{
    internal class Snake
    {
        public Pixel Head { get; set; }
        public List<Pixel> Body { get; set; }

        public Snake() { }

        public Snake (int postionX, int positionY)
        {
            Head = new Pixel()
            {
                PositionX = postionX,
                PositionY = positionY,
                Schermkleur = ConsoleColor.DarkRed
            };

            Body = new List<Pixel>();
        }
    }
}
