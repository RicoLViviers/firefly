using Firefly.Sandbox;
using OpenTK.Windowing.Desktop;


internal static class Program
{
    static void Main()
    {
        NativeWindowSettings windowSettings = new()
        {
            ClientSize = new OpenTK.Mathematics.Vector2i(800, 600),
            Title = "Anti-Aliasing Example",
            // Set the number of MSAA samples (e.g., 4 or 8)
            NumberOfSamples = 4
        };


        using Sandbox sandbox = new(GameWindowSettings.Default, windowSettings);
        sandbox.Run();
    }
}