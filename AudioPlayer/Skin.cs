using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer
{
    public abstract class Skin
    {
        public abstract void NewScreen();

        public abstract void Render(string text, ConsoleColor backgroundColor, ConsoleColor foregroundColor);

        public abstract void Render(string[] text, ConsoleColor backgroundColor, ConsoleColor foregroundColor);
    }

    public class ClassicSkin : Skin
    {
        public override void NewScreen()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
        }

        public override void Render(string text, ConsoleColor backgroundColor = ConsoleColor.Black, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(text);
        }

        public override void Render(string[] text, ConsoleColor backgroundColor = ConsoleColor.Black, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;

            for (int i = 0; i < text.Length; i++)
            {
                Console.Write(i + " ");
            }
        }
    }

    public class DOSSkin : Skin
    {
        public override void NewScreen()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
        }

        public override void Render(string text, ConsoleColor backgroundColor = ConsoleColor.Blue, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(text);
        }

        public override void Render(string[] text, ConsoleColor backgroundColor = ConsoleColor.Blue, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;

            for (int i = 0; i < text.Length; i++)
            {
                Console.Write(i + " ");
            }
        }
    }
}