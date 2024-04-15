namespace SnakeApp.Models
{
    internal class Configuration
    {
        /// <summary>
        /// Width of console window
        /// </summary>
        public int WindowWidth { get; set; }

        /// <summary>
        /// Height of console window
        /// </summary>
        public int WindowHeight { get; set; }

        /// <summary>
        /// Interval for waiting for key press [ms]
        /// </summary>
        public int KeyPressedInterval { get; set; }
    }
}
