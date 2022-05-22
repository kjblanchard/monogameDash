using System;
using SupergoonDashCrossPlatform;

namespace SupergoonDashDesktop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}