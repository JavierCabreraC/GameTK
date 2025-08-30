namespace GameTK
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new Game(800, 640))
            {
                game.Run(60.0);
            }
        }
    }
}
