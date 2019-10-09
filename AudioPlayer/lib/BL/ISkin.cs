using System;

namespace AudioPlayer.lib.BL
{
    internal interface ISkin
    {
        void NewScreen();

        void Render(string text, ConsoleColor color = ConsoleColor.White);

        void Render(string[] text, ConsoleColor color = ConsoleColor.White);
    }
}
