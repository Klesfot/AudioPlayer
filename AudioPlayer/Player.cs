using System;
using System.Collections.Generic;
using ExtensionMethods;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Media;

namespace AudioPlayer
{
    class Player : GenericPlayer, IDisposable
    {
        public Player(ColorSkin skin)
        {
            this.currentSkin = skin;
        }
        public enum Genre : int
        {
            PsyTrance = 0,
            Electronic = 1,
            Hardcore = 2,
            DnB = 3,
            Drumstep = 4,
            NaN = 5
        };

        public Playlist playlist = new Playlist();
        private int _maxVolume = 100, _minVolume = 0;
        private int _volume;
        private SoundPlayer currentPlayer;
        private bool isDisposed = false;

        public ColorSkin currentSkin = null;

        public int Volume
        {
            get
            {
                return _volume;
            }

            set
            {
                if (value > _maxVolume)
                    _volume = _maxVolume;

                else if (value < 0)
                    _volume = _minVolume;

                else
                    _volume = value;
            }
        }

        private bool playing = true;

        public bool Playing
        {
            get
            {
                return playing;
            }
        }

        bool IsLocked, IsOnLoop;


        public void Play(bool IsOnLoop = false)
        {
            if (playing == true)
            {
                if (IsLocked == false)
                {
                    if (IsOnLoop == false)
                    {
                        for (int i = 0; i < playlist.Songs.Count; i++)
                        {
                            Genre genre = (Genre)playlist.Songs[i].Genre;
                            if (playlist.Songs[i].IsLiked == true)
                            {
                                printSong(i, genre, "green");
                                SoundPlayer truePlayer = new SoundPlayer(playlist.Songs[i].Path);
                                truePlayer.PlaySync();
                                currentPlayer = truePlayer;
                            }

                            else if (playlist.Songs[i].IsLiked == false)
                            {
                                printSong(i, genre, "red");
                                SoundPlayer truePlayer = new SoundPlayer(playlist.Songs[i].Path);
                                truePlayer.PlaySync();
                                currentPlayer = truePlayer;
                            }

                            else
                            {
                                printSong(i, genre);
                                SoundPlayer truePlayer = new SoundPlayer(playlist.Songs[i].Path);
                                truePlayer.PlaySync();
                                currentPlayer = truePlayer;
                            }
                        }
                    }

                    else
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Genre genre = (Genre)playlist.Songs[i].Genre;
                            if (playlist.Songs[i].IsLiked == true)
                            {
                                printSong(i, genre, "green");
                                SoundPlayer truePlayer = new SoundPlayer(playlist.Songs[i].Path);
                                truePlayer.PlaySync();
                                currentPlayer = truePlayer;
                            }

                            else if (playlist.Songs[i].IsLiked == false)
                            {
                                printSong(i, genre, "red");
                                SoundPlayer truePlayer = new SoundPlayer(playlist.Songs[i].Path);
                                truePlayer.PlaySync();
                                currentPlayer = truePlayer;
                            }

                            else
                            {
                                printSong(i, genre);
                                SoundPlayer truePlayer = new SoundPlayer(playlist.Songs[i].Path);
                                truePlayer.PlaySync();
                                currentPlayer = truePlayer;
                            }

                        }
                    }
                }

                else
                    currentSkin.Render("Player is locked, unlock it first");
            }

            else if (playing == false)
                currentSkin.Render("Player has not started, start it first");
        }


        public bool Start()
        {
            if (IsLocked == false)
            {
                playing = true;
                currentSkin.Render("Player has started");
            }

            else
            {
                currentSkin.Render("Player is locked, unlock first");
            }

            return playing;
        }


        public bool Stop()
        {
            if (IsLocked == false)
            {
                playing = false;
                currentSkin.Render("Player has stopped");
            }

            else
            {
                currentSkin.Render("Player is locked, unlock first");

            }

            return playing;
        }


        public void Lock()
        {
            IsLocked = true;
            currentSkin.Render("Player is locked");
        }


        public void Unlock()
        {
            IsLocked = false;
            currentSkin.Render("Player is unlocked");
        }


        public void VolumeUp()
        {
            Volume += 5;
            currentSkin.Render("Volume is: " + Volume);
        }


        public void VolumeDown()
        {
            Volume -= 5;
            currentSkin.Render("Volume is: " + Volume);
        }


        public int VolumeChange(int amount)
        {
            Volume = amount;
            currentSkin.Render("Volume is: " + Volume);

            return 0;
        }


        public void Load(string path)
        {
            List<Song> songs = new List<Song>();
            DirectoryInfo dir = new DirectoryInfo(path);

            foreach (var item in dir.EnumerateFiles())
            {
                Song song = new Song((int)item.Length / 1000,
                                          item.Name,
                                          item.FullName);
                song.Artist = new Artist("Test artist", "None", "Sweden");
                songs.Add(song);
            }

            playlist.Songs.AddRange(songs);
        }


        public void Clear()
        {
            playlist.Songs.Clear();
        }


        public void PrintPlaylist(List<Song> songs)
        {
            for (int i = 0; i < songs.Count; i++)
            {
                songs[i].Title = CutToDots(songs[i].Title);
                Genre genre = (Genre)songs[i].Genre;
                currentSkin.Render(songs[i].Artist.Name + " " + songs[i].Title + " " + songs[i].Duration + " " + genre);
            }
        }


        public List<Song> Shuffle()
        {
            this.playlist.Songs = this.playlist.Songs.Shuffle();
            return this.playlist.Songs;
        }


        public List<Song> SortByTitle()
        {
            this.playlist.Songs = this.playlist.Songs.SortByTitle();
            return this.playlist.Songs;
        }


        public List<Song> FilterByGenre(int input)
        {
            this.playlist.Songs = this.playlist.Songs.FilterByGenre(input);
            return this.playlist.Songs;
        }


        public void printSong(int i, Genre genre = Genre.NaN, string color = "white")
        {
            if (color == "green")
            {
                currentSkin.Render(CutToDots(playlist.Songs[i].Title) + " " + playlist.Songs[i].Artist.Name +
                                                            " " + playlist.Songs[i].Duration +
                                                            " " + genre, ConsoleColor.Green);
                System.Threading.Thread.Sleep(playlist.Songs[i].Duration);
            }
            else if (color == "red")
            {
                currentSkin.Render(CutToDots(playlist.Songs[i].Title) + " " + playlist.Songs[i].Artist.Name +
                                                            " " + playlist.Songs[i].Duration +
                                                            " " + genre, ConsoleColor.Red);
                System.Threading.Thread.Sleep(playlist.Songs[i].Duration);
            }
            else
            {
                currentSkin.Render(CutToDots(playlist.Songs[i].Title) + " " + playlist.Songs[i].Artist.Name +
                                                           " " + playlist.Songs[i].Duration +
                                                           " " + genre);
                System.Threading.Thread.Sleep(playlist.Songs[i].Duration);
            }
        }

        public string CutToDots(string data)
        {
            data = data.CutToDots();
            return data;
        }


        public void SaveCurrentPlaylist(string path = @"D:\Songs\SavedPlaylist.xml")
        {
            List<Song> songs = playlist.Songs;
            string result = "";
            foreach (var item in songs)
            {
                string temp;
                XmlSerializer ser = new XmlSerializer(item.GetType());
                using (var sww = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        ser.Serialize(writer, item);
                        temp = sww.ToString();
                        result += temp;
                    }
                }
            }

            FileInfo file = new FileInfo(path);

            if (file.Exists)
                file.Delete();
            else if (!file.Exists)
                file.Create();
            StreamWriter sw = file.AppendText();
            sw.WriteLine(result);
        }


        public void LoadPlaylist(string path = @"D:\Songs\SavedPlaylist.xml")
        {
            List<Song> temp;
            XmlSerializer ser = new XmlSerializer(typeof(Song));
            string xmlString = File.ReadAllText(path);

            using (TextReader reader = new StringReader(xmlString))
            {
                temp = (List<Song>)ser.Deserialize(reader);
            }

            playlist.Songs.AddRange(temp);
        }


        ~Player()
        {
            if (isDisposed == false)
                Dispose();
        }


        public void Dispose()
        {
            currentSkin = null;
            playlist.Dispose();
            isDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}