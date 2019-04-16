using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace AudioPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            int min, max, total = 0;
            var player = new Player();
            var songs = CreateSongs(out min, out max, ref total);
            player.Songs = songs;

            Console.WriteLine($"Total duration: " + total + " max duration: " + max + " min duration: " + min);

            while (true)
            {
                switch (ReadLine())
                {
                    case "U":
                        {
                            player.VolumeUp();
                        }
                        break;

                    case "D":
                        {
                            player.VolumeDown();
                        }
                        break;

                    case "P":
                        {
                            player.Play();
                        }
                        break;

                    case "Volume":
                        {
                            Console.WriteLine("Specify volume: ");
                            int inputAmount = Convert.ToInt32(Console.ReadLine());
                            player.VolumeChange(inputAmount);
                        }
                        break;

                    case "Start":
                        {
                            player.Start();
                        }
                        break;

                    case "Stop":
                        {
                            player.Stop();
                        }
                        break;

                    case "Lock":
                        {
                            player.Lock();
                        }
                        break;

                    case "Unlock":
                        {
                            player.Unlock();
                        }
                        break;
                }
            }
        }

        private static Song[] CreateSongs(out int min, out int max, ref int total)
        {
            Random rand = new Random();
            Song[] songs = new Song[5];
            int MinDuration = int.MaxValue, MaxDuration = int.MinValue, TotalDuration = 0;

            for (int i = 0; i < songs.Length; i++)
            {
                var song1 = new Song();
                song1.Title = "Song" + i;
                song1.Duration = rand.Next(3001);
                song1.Artist = new Artist();
                songs[i] = song1;
                TotalDuration += song1.Duration;
                MinDuration = song1.Duration < MinDuration ? song1.Duration : MinDuration;
                MaxDuration = song1.Duration > MaxDuration ? song1.Duration : MaxDuration;
            }

            total = TotalDuration;
            max = MaxDuration;
            min = MinDuration;

            return songs;
        }
    }
}
