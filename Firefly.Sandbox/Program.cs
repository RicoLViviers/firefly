using Firefly.Sandbox;
using OpenTK.Windowing.Desktop;


internal static class Program
{
    static void Main()
    {
        NativeWindowSettings windowSettings = new()
        {
            ClientSize = new OpenTK.Mathematics.Vector2i(800, 600),
            Title = "Sandbox"
        };


        using Sandbox sandbox = new(GameWindowSettings.Default, windowSettings);
        sandbox.Run();
    }
}