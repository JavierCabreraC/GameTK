using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

public class Program
{
    static void Main(string[] args)
    {
        var settings = GameWindowSettings.Default;
        var nativeSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(1200, 800),
            Title = "Setup de Computadora",
            APIVersion = new Version(3, 3),
            Flags = ContextFlags.Default,
            Profile = ContextProfile.Compatability
        };

        using var game = new Game(settings, nativeSettings);
        game.Run();
    }
}