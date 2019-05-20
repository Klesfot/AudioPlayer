using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer
{
    public interface ISkin
    {
        void NewScreen();

        void Render(string text, ConsoleColor color);

        void Render(string[] text, ConsoleColor color);
    }

    public class ColorSkin : ISkin
    {
        ConsoleColor BGColor;
        ConsoleColor FGColor;

        public ColorSkin(ConsoleColor bgColor = ConsoleColor.Black, ConsoleColor fgColor = ConsoleColor.White)
        {
            this.BGColor = bgColor;
            this.FGColor = fgColor;
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