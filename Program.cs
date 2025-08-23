namespace GameTK;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        using(Game game = new Game())
        {
            game.Run();
        }
    }
}