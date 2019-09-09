using System;
using static System.Console;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using ExtensionMethods;
using System.Collections.Generic;

namespace AudioPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            var defaultSkin = new ColorSkin();

            using (var player = new Player(defaultSkin))
            {
                player.PlayerStartedEvent += (bool isPlaying) =>
                {
                    Visualise(player, player.PlayingPlaylist);
                };

                player.PlayerStoppedEvent += (bool isPlaying) =>
                {
                    List<Song> clearList = new List<Song>();
                    Visualise(player, clearList);
                };

                player.PlayerLockedEvent += (bool isLocked) =>
                {
                    Visualise(player, player.PlayingPlaylist);
                };

                player.PlayerUnlockedEvent += (bool isLocked) =>
                {
                    Visualise(player, player.PlayingPlaylist);
                };

                player.VolumeChangedEvent += (float inputAmount) =>
                {
                    Visualise(player, player.PlayingPlaylist);
                };

                player.SongStartedEvent += (Song song) =>
                {
                    Visualise(player, player.PlayingPlaylist);
                };

                player.SongStoppedEvent += (Song song) =>
                {
                    Visualise(player, player.PlayingPlaylist);
                };

                player.PlaybackAbortedEvent += (bool isAborted) =>
                {
                    Visualise(player, player.PlayingPlaylist);
                };

                while (true)
                {
                    switch (ReadLine())
                    {
                        case "STX":
                        {
                            player.currentSkin.Render("Please input index of a song to serialize to string format: ");
                            int input = Convert.ToInt32(Console.ReadLine());
                            SerXML(player.playlist.Songs[input]);
                        }
                        break;

                        case "DSTX":
                        {
                            player.currentSkin.Render("Please input index of a song to deserialize to string format: ");
                            int input = Convert.ToInt32(Console.ReadLine());
                            Song temp = DeSerXML(SerXML(player.playlist.Songs[input]));
                            Player.Genre genre = (Player.Genre)temp.Genre;
                            player.currentSkin.Render(temp.Title + " " + temp.Duration + " " + genre);
                        }
                        break;

                        case "SFX":
                        {
                            player.currentSkin.Render("Please input index of a song to serialize to file: ");
                            int input = Convert.ToInt32(Console.ReadLine());
                            SerXMLF(player.playlist.Songs[input]);
                        }
                        break;

                        case "DSFX":
                        {
                            player.currentSkin.Render("Please input index of a song to serialize to file: ");
                            int input = Convert.ToInt32(Console.ReadLine());
                            SerXMLF(player.playlist.Songs[input]);
                            Song temp = DeSerXMLF(@"D:\NewXML.txt");
                            Player.Genre genre = (Player.Genre)temp.Genre;
                            player.currentSkin.Render(temp.Title + " " + temp.Duration + " " + genre);
                        }
                        break;

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
                            if (player.IsPlaying == false)
                                player.Start();
                            player.Play(true);
                        }
                        break;

                        case "-Loop":
                        {
                            player.IsOnLoop = false;
                        }
                        break;

                        case "P":
                        {
                            if (player.IsPlaying == false)
                                player.Start();
                            player.Play();
                        }
                        break;

                        case "V":
                        {
                            player.currentSkin.Render("Specify volume: ");
                            float inputAmount = Convert.ToSingle(Console.ReadLine().Replace('.',','));
                            player.VolumeChange(inputAmount);
                        }
                        break;

                        case "Start":
                        {
                            player.Start();
                        }
                        break;

                        case "s":
                        {
                            player.Stop(player.currentPlaybackDevice);
                        }
                        break;

                        case "a":
                        {
                            player.Abort();
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

                        case "Load":
                        {
                            player.currentSkin.Render("Please enter the path to desired directory");
                            string path = Console.ReadLine();
                            player.Load(path);
                        }
                        break;

                        case "Clear":
                        {
                            player.playlist.Songs.ClearAll();
                        }
                        break;

                        case "Shuffle":
                        {
                            player.playlist.Songs = player.Shuffle();
                        }
                        break;

                        case "SortT":
                        {
                            player.playlist.Songs = player.SortByTitle();
                        }
                        break;

                        case "+":
                        {
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

                        case "SaveP":
                        {
                            player.SaveCurrentPlaylist();
                        }
                        break;

                        case "LoadP":
                        {
                            player.LoadPlaylist();
                        }
                        break;

                        case "Exit":
                        {
                            player.Dispose();
                        }
                        break;
                    }
                }
            }
        }


        private static void Visualise(Player player, List<Song> songs)
        {
            PrintCurrentPlaylist(player, songs);
            try
            {
                if (player.IsPlaying == false && player.PlayingSong == null)
                    player.currentSkin.Render("Now playing: ");
                else
                    player.currentSkin.Render($"Now playing: {player.PlayingSong.Title}");
                player.currentSkin.Render("---------------------------------------------------------------------------");
                player.currentSkin.Render($"Player playing = {player.IsPlaying}" + " " + $", volume is: {player.Volume}" + " " + $", player locked = {player.IsLocked}" + " " + $", player is on loop = {player.IsOnLoop}");
            }

            catch (NullReferenceException)
            {
                player.currentSkin.Render($"Now playing: ");
                player.currentSkin.Render("---------------------------------------------------------------------------");
                player.currentSkin.Render($"Player playing = {player.IsPlaying}" + " " + $", volume is: {player.Volume}" + " " + $", player locked = {player.IsLocked}" + " " + $", player is on loop = {player.IsOnLoop}");
            }
        }


        private static void PrintCurrentPlaylist(Player player, List<Song> songs)
        {
            player.currentSkin.NewScreen();
            foreach (var item in songs)
            {
                if (item == player.PlayingSong)
                    player.currentSkin.Render(item.Title.CutToDots() + " " + item.Artist + " " + item.Duration, ConsoleColor.Gray);
                else if (item.IsLiked == false)
                    player.currentSkin.Render(item.Title.CutToDots() + " " + item.Artist + " " + item.Duration, ConsoleColor.Red);
                else if (item.IsLiked == true)
                    player.currentSkin.Render(item.Title.CutToDots() + " " + item.Artist + " " + item.Duration, ConsoleColor.Green);
                else
                    player.currentSkin.Render(item.Title.CutToDots() + " " + item.Artist + " " + item.Duration);
            }
        }

        private static string SerXML(Song song)
        {
            string temp;
            XmlSerializer ser = new XmlSerializer(song.GetType());
            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    ser.Serialize(writer, song);
                    temp = sww.ToString();
                    return temp;
                }
            }
        }


        private static Song DeSerXML(string xmlString)
        {
            Song temp;
            XmlSerializer ser = new XmlSerializer(typeof(Song));

            using (TextReader reader = new StringReader(xmlString))
            {
                temp = (Song)ser.Deserialize(reader);
                return temp;
            }
        }


        private static string SerXMLF(Song song)
        {
            string path = @"D:\NewXML.txt";
            if (File.Exists(path))
                File.Delete(path);
            else if (!File.Exists(path))
                File.Create(path);
            File.AppendAllText(path, SerXML(song));
            return path;
        }

        private static Song DeSerXMLF(string path)
        {
            Song temp;
            XmlSerializer ser = new XmlSerializer(typeof(Song));
            string xmlString = File.ReadAllText(path);

            using (TextReader reader = new StringReader(xmlString))
            {
                temp = (Song)ser.Deserialize(reader);
                return temp;
            }
        }
    }
}