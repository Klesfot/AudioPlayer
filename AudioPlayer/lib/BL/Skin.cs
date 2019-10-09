using System;

namespace AudioPlayer.lib.BL
{
    public class ColorSkin : ISkin
    {
        ConsoleColor BGColor;
        ConsoleColor FGColor;

        public ColorSkin(ConsoleColor bgColor = ConsoleColor.Black, ConsoleColor fgColor = ConsoleColor.White)
        {
            BGColor = bgColor;
            FGColor = fgColor;
        }

        public void NewScreen()
        {
            Console.BackgroundColor = BGColor;
            Console.Clear();
            Console.ForegroundColor = FGColor;
        }

        public void Render(string text, ConsoleColor customFGColor = ConsoleColor.White)
        {
            Console.BackgroundColor = BGColor;
            Console.ForegroundColor = customFGColor;
            Console.WriteLine(text);
        }

        public void Render(string[] text, ConsoleColor customFGColor = ConsoleColor.White)
        {
            Console.BackgroundColor = BGColor;
            Console.ForegroundColor = customFGColor;

            for (int i = 0; i < text.Length; i++)
            {
                Console.Write(text[i] + " ");
            }
        }
    }
}