using System;
using System.Collections.Generic;
using static System.Console;

namespace AudioPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            int min, max, total = 0;

            var defaultSkin = new ColorSkin();

            var player = new Player(defaultSkin);
            var songs = CreateSongs(out min, out max, ref total);

            while (true)
            {
                switch (ReadLine())
                {
                    case "u":
                    {
                        player.VolumeUp();
                    }
                    break;

                    case "d":
                    {
                        player.VolumeDown();
                    }
                    break;

                    case "Loop":
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
                        player.currentSkin.Render("Specify volume: ");
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

                    case "Add":
                    {
                        player.Add(songs);
                    }
                    break;

                    case "Shuffle":
                    {
                        player.playlist.Songs = player.Shuffle();
                        player.PrintPlaylist(player.playlist.Songs);
                    }
                    break;

                    case "SortT":
                    {
                        player.playlist.Songs = player.SortByTitle();
                        player.PrintPlaylist(player.playlist.Songs);
                    }
                    break;

                    case "+":
                    {
                        player.currentSkin.Render("Please specify the title of a song in current playlist that you wish to like");
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
                        player.currentSkin.Render("Please specify the the title of a song in current playlist that you wish to like");
                        var input = Console.ReadLine();

                        for (int i = 0; i < player.playlist.Songs.Count; i++)
                        {
                            if (player.playlist.Songs[i].Title == input)
                                player.playlist.Songs[i].Dislike();
                        }
                    }
                    break;

                    case "SortG":
                    {
                        player.currentSkin.Render("Please specify genre");
                        string input = Console.ReadLine();
                        int inputInt = 0;
                        string[] Genre = { "PsyTrance", "Electronic", "Hardcore", "DnB", "Drumstep" };

                        for (int i = 0; i < 5; i++)
                        {
                            if (input == Genre[i])
                            {
                                inputInt = i;
                            }
                        }

                        player.playlist.Songs = player.FilterByGenre(inputInt);
                        player.PrintPlaylist(player.playlist.Songs);
                    }
                    break;

                    case "Test":
                    {
                        player.currentSkin.Render("Artist, duration, title");
                        string input = Console.ReadLine();
                        string[] inputArr = input.Split(',');

                        int duration = 0;
                        string title = "";
                        Artist artist = new Artist();

                        for (int i = 0; i < inputArr.Length; i++)
                        {
                            if (i == 0)
                                artist.Name = inputArr[i];

                            if (i == 1)
                                duration = Convert.ToInt32(inputArr[i]);

                            if (i == 2)
                                title = inputArr[i];
                        }

                        player.Add(CreateTestSong(artist, duration, title));
                    }
                    break;

                    case "s 0":
                    {
                        var tempSkin = new ColorSkin();
                        player.currentSkin = tempSkin;
                        player.currentSkin.NewScreen();
                    }
                    break;

                    case "s 1":
                    {
                        var tempSkin = new ColorSkin(ConsoleColor.Blue);
                        player.currentSkin = tempSkin;
                        player.currentSkin.NewScreen();
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
                var title = "Song" + i;
                var duration = rand.Next(3001);
                var artist = new Artist();
                var genre = rand.Next(5);
                var song1 = new Song(duration, title, artist, genre);

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


        private static Song CreateTestSong(Artist artist, int duration, string title)
        {
            var song = new Song(duration, title, artist);
            return song;
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