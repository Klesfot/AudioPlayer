using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;

namespace AudioPlayer
{
    class Player
    {
        public Player(int skin = 0)
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
        public int currentSkin = 0;

        public ClassicSkin classicSkin = new ClassicSkin();
        public DOSSkin dosSkin = new DOSSkin();

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
                            }

                            else if (playlist.Songs[i].IsLiked == false)
                            {
                                printSong(i, genre, "red");
                            }

                            else
                            {
                                printSong(i, genre);
                            }
                        }
                    }

                    else
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Genre genre = (Genre)playlist.Songs[i].Genre;
                            if (playlist.Songs[i].IsLiked == true)
                                printSong(i, genre, "green");

                            else if (playlist.Songs[i].IsLiked == false)
                            {
                                printSong(i, genre, "red");
                            }

                            else
                            {
                                printSong(i, genre);
                            }

                        }
                    }
                }

                else
                    Render("Player is locked, unlock it first");
            }

            else if (playing == false)
                Render("Player has not started, start it first");
        }


        public bool Start()
        {
            if (IsLocked == false)
            {
                playing = true;
                Render("Player has started");
            }

            else
            {
                Render("Player is locked, unlock first");
            }

            return playing;
        }


        public bool Stop()
        {
            if (IsLocked == false)
            {
                playing = false;
                Render("Player has stopped");
            }

            else
            {
                Render("Player is locked, unlock first");

            }

            return playing;
        }


        public void Lock()
        {
            IsLocked = true;
            Render("Player is locked");
        }


        public void Unlock()
        {
            IsLocked = false;
            Render("Player is unlocked");
        }


        public void VolumeUp()
        {
            Volume += 5;
            Render("Volume is: " + Volume);
        }


        public void VolumeDown()
        {
            Volume -= 5;
            Render("Volume is: " + Volume);
        }


        public int VolumeChange(int amount)
        {
            Volume = amount;
            Render("Volume is: " + Volume);

            return 0;
        }

        public void Add(Song song1)
        {
            playlist.Songs.Add(song1);
            Genre genre = (Genre)song1.Genre;
            song1.Title = CutToDots(song1.Title);
            Render("Added song: " + " " + song1.Title + " " + song1.Artist.Name + " " + song1.Duration + " " + genre);
        }

        public void Add(List<Song> songs)
        {
            for (int i = 0; i < songs.Capacity; i++)
            {
                (var duration, var title, var artist) = songs[i];
                Genre genre = (Genre)songs[i].Genre;
                playlist.Songs.Add(songs[i]);
                songs[i].Title = CutToDots(title);
                Render("Added song: " + " " + title + " " + artist.Name +
                                        " " + duration + " " + genre);
            }
        }


        public void PrintPlaylist(List<Song> songs)
        {
            for (int i = 0; i < songs.Count; i++)
            {
                songs[i].Title = CutToDots(songs[i].Title);
                Genre genre = (Genre)songs[i].Genre;
                Render(songs[i].Artist.Name + " " + songs[i].Title + " " + songs[i].Duration + " " + genre);
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


        public void printSong(int i, Genre genre, string color = "white")
        {
            if (color == "green")
            {
                Render(CutToDots(playlist.Songs[i].Title) + " " + playlist.Songs[i].Artist.Name +
                                                            " " + playlist.Songs[i].Duration +
                                                            " " + genre, ConsoleColor.Green);
                System.Threading.Thread.Sleep(playlist.Songs[i].Duration);
            }
            else if (color == "red")
            {
                Render(CutToDots(playlist.Songs[i].Title) + " " + playlist.Songs[i].Artist.Name +
                                                            " " + playlist.Songs[i].Duration +
                                                            " " + genre, ConsoleColor.Red);
                System.Threading.Thread.Sleep(playlist.Songs[i].Duration);
            }
            else
            {
                Render(CutToDots(playlist.Songs[i].Title) + " " + playlist.Songs[i].Artist.Name +
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

        public void Render(string text, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black)
        {
            if (currentSkin == 1)
                bgColor = ConsoleColor.Blue;

            NewScreen();
            if (currentSkin == 0)
            {
                classicSkin.Render(text, bgColor, fgColor);
            }

            else if (currentSkin == 1)
            {
                dosSkin.Render(text, bgColor, fgColor);
            }
        }

        public void Render(string[] text, ConsoleColor bgColor = ConsoleColor.Black, ConsoleColor fgColor = ConsoleColor.White)
        {
            if (currentSkin == 1)
                bgColor = ConsoleColor.Blue;

            NewScreen();
            if (currentSkin == 0)
            {
                classicSkin.Render(text, bgColor, fgColor);
            }

            else if (currentSkin == 1)
            {
                dosSkin.Render(text, bgColor, fgColor);
            }
        }


        public void NewScreen()
        {
            if (currentSkin == 0)
            {
                classicSkin.NewScreen();
            }

            else if (currentSkin == 1)
            {
                dosSkin.NewScreen();
            }
        }
    }
}