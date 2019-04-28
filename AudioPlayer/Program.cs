using System;
using System.Collections.Generic;
using System.Collections;
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

            Song song2 = CreateNamedSong("Election");

            Artist Artist3 = new Artist("Xi");
            Song song3 = CreateSong("Zauberkugel", Artist3.Name, 3000);
            
            Artist Artist2 = AddArtist("Infected mushroom");
            Artist Artist1 = AddArtist();
            Album Album1 = AddAlbum("Seventh moon", "1994");
            Album Album2 = AddAlbum();
            Album Album3 = AddAlbum("1080", "Spicy ooze");

            Console.WriteLine("Artist 2 is: " + " " + Artist2.Name + " " + Artist2.Nickname + " " + Artist3.Country);
            Console.WriteLine("Artist 1 is: " + " " + Artist1.Name + " " + Artist1.Nickname + " " + Artist1.Country);
            Console.WriteLine("Album 1 is: " + " " + Album1.Name + " " + Album1.Year);
            Console.WriteLine("Album 2 is: " + " " + Album2.Name + " " + Album2.Year);
            Console.WriteLine("Album 3 is: " + " " + Album3.Name + " " + Album3.Year);

            var player = new Player();
            var songs = CreateSongs(out min, out max, ref total);
            var sortList = new List<Song>();
            sortList = songs;

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

                    case "Pt":
                    {
                        player.Play(true);
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

                    case "AddArr":
                    {
                        player.Add(songs);
                    }
                    break;

                    case "Shuffle":
                    {
                        player.playlist.Songs = player.Shuffle(player.playlist.Songs);
                        player.PrintPlaylist(player.playlist.Songs);
                    }
                    break;

                    case "SortT":
                    {
                        sortList = player.SortByTitle(sortList);
                        player.playlist.Songs.Clear();
                        player.playlist.Songs = sortList;
                        player.PrintPlaylist(player.playlist.Songs);
                    }
                    break;

                    case "+":
                    {
                        Console.WriteLine("Please specify the title of a song in current playlist that you wish to like");
                        var input = Console.ReadLine();

                        for (int i = 0; i < player.playlist.Songs.Count; i++)
                        {
                            if (player.playlist.Songs[i].Title == input)
                                player.playlist.Songs[i].Like();
                        }
                    }
                    break;

                    case "-":
                    {
                        Console.WriteLine("Please specify the the title of a song in current playlist that you wish to like");
                        var input = Console.ReadLine();

                        for (int i = 0; i < player.playlist.Songs.Count; i++)
                        {
                            if (player.playlist.Songs[i].Title == input)
                                player.playlist.Songs[i].Dislike();
                        }
                    }
                    break;
                }
            }
        }


        private static List<Song> CreateSongs(out int min, out int max, ref int total)
        {
            Random rand = new Random();
            List<Song> songs = new List<Song>(5);
            int MinDuration = int.MaxValue, MaxDuration = int.MinValue, TotalDuration = 0;

            for (int i = 0; i < songs.Capacity; i++)
            {
                var song1 = new Song();
                song1.Title = "Song" + i;
                song1.Duration = rand.Next(3001);
                song1.Artist = new Artist();
                song1.Genre = rand.Next(5);
                songs.Add(song1);
                TotalDuration += song1.Duration;
                MinDuration = song1.Duration < MinDuration ? song1.Duration : MinDuration;
                MaxDuration = song1.Duration > MaxDuration ? song1.Duration : MaxDuration;
            }

            total = TotalDuration;
            max = MaxDuration;
            min = MinDuration;

            return songs;
        }


		private static Song CreateDefaultSong()
        {
            Random rand = new Random();
            var song1 = new Song();

            song1.Title = Convert.ToString(rand.Next(1000));
            song1.Duration = rand.Next(3001);
            song1.Artist = new Artist(Convert.ToString(rand.Next(3001)));
            return song1;
        }


        private static Song CreateNamedSong(string name)
        {
            Random rand = new Random();
            
            var song2 = new Song();
            song2.Title = name;
            song2.Duration = rand.Next(3001);
            song2.Artist = new Artist(Convert.ToString(rand.Next(3001)));
            return song2;
        }


        private static Song CreateSong(string name, string artistName, int duration)
        {
            Random rand = new Random();

            var song3 = new Song();
            song3.Title = name;
            song3.Duration = duration;
            song3.Artist = new Artist(artistName);
            return song3;
        }


        private static Artist AddArtist(string artistName = "Unknown artist", string artistNick = "N/A", string artistCountry = "N/A")
        {
            Artist artist1 = new Artist();
            artist1.Name = artistName;
            artist1.Nickname = artistNick;
            artist1.Country = artistCountry;
            return artist1;
        }


        private static Album AddAlbum(string albumName = "Unknown album", string albumYear = "-")
        {
            Album album1 = new Album();
            album1.Name = albumName;
            album1.Year = albumYear;
            return album1;
        }
    }
}
