using Firefly.Editor;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;

internal static class Program
{
    static void Main()
    {
        NativeWindowSettings windowSettings = new()
        {
            StartVisible = false,
            ClientSize = new OpenTK.Mathematics.Vector2i(800, 600),
            Title = "Anti-Aliasing Example",
            NumberOfSamples = 4
        };


        using Editor editor = new(GameWindowSettings.Default, windowSettings);
        editor.IsVisible = true;
        editor.Run();
    }
}