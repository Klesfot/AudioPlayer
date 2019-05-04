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
        public enum Genre : int
        {
            PsyTrance = 0,
            Electronic = 1,
            Hardcore = 2,
            DnB = 3,
            Drumstep = 4
        };
        
        public Playlist playlist = new Playlist();
        private int _maxVolume = 100, _minVolume = 0;
        private int _volume;

        public int Volume
        {
            get
            {
                return _volume;
            }

            set
            {
                if(value > _maxVolume)
                    _volume = _maxVolume;

                else if(value < 0)
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
                        for (int l = 0; l < 5; l++)
                        {
                            for (int i = 0; i < playlist.Songs[i].Duration; i++)
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
                }
                else
                    Console.WriteLine("Player is locked, unlock it first");
            }
            else if (playing == false)
                Console.WriteLine("Player has not started, start it first");
        }


        public bool Start()
        {
            if (IsLocked == false)
            {
                playing = true;
                Console.WriteLine("Player has started");
            }

            else
            {
                Console.WriteLine("Player is locked, unlock first");
            }

            return playing;
        }


        public bool Stop()
        {
            if (IsLocked == false)
            {
                playing = false;
                Console.WriteLine("Player has stopped");
            }

            else
            {
                Console.WriteLine("Player is locked, unlock first");

            }

            return playing;
        }


        public void Lock()
        {
            IsLocked = true;
            Console.WriteLine("Player is locked");
        }


        public void Unlock()
        {
            IsLocked = false;
            Console.WriteLine("Player is unlocked");
        }


        public void VolumeUp()
        {
            Volume += 5;
            Console.WriteLine("Volume is: " + Volume);
        }


        public void VolumeDown()
        {
            Volume -= 5;
            Console.WriteLine("Volume is: " + Volume);
        }


        public int VolumeChange(int amount)
        {
            Volume = amount;
            Console.WriteLine("Volume is: " + Volume);

            return 0;
        }


        private void SongsParamsList(params string[] Songs)
        {
            Console.WriteLine(Songs);
        }


        public void Add(Song song1)
        {
            playlist.Songs.Add(song1);
            Genre genre = (Genre)song1.Genre;
            song1.Title = CutToDots(song1.Title);
            Console.WriteLine("Added song: " + " " + song1.Title + " " + song1.Artist.Name + " " + song1.Duration + " " + genre);
        }


        public Song[] Add(Song song1, Song song2)
        {
            playlist.Songs.Add(song1);
            playlist.Songs.Add(song2);

            Genre genre1 = (Genre)song1.Genre;
            Genre genre2 = (Genre)song2.Genre;

            song1.Title = CutToDots(song1.Title);
            song2.Title = CutToDots(song2.Title);

            Console.WriteLine("Added song: " + " " + song1.Title + " " + song1.Artist.Name + " " + song1.Duration + " " + genre1);
            Console.WriteLine("Added song: " + " " + song2.Title + " " + song2.Artist.Name + " " + song2.Duration + " " + genre2);
            return null;
        }


        public void Add(List<Song> songs)
        {
            for (int i = 0; i < songs.Capacity; i++)
            {
                (var duration, var title, var artist) = songs[i];

                Genre genre = (Genre)songs[i].Genre;
                playlist.Songs.Add(songs[i]);

                songs[i].Title = CutToDots(title);

                Console.WriteLine("Added song: " + " " + title + " " + artist.Name + 
                                                   " " + duration + " " + genre);
            }
        }


        public void PrintPlaylist(List<Song> songs)
        {
            Console.WriteLine("Playlist contains following songs: ");
            for (int i = 0; i < songs.Count; i++)
            {
                songs[i].Title = CutToDots(songs[i].Title);
                Genre genre = (Genre)songs[i].Genre;
                Console.WriteLine(songs[i].Artist.Name + " " + songs[i].Title + " " + songs[i].Duration + " " + genre);
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(CutToDots(playlist.Songs[i].Title) + " " + playlist.Songs[i].Artist.Name +
                                                            " " + playlist.Songs[i].Duration +
                                                            " " + genre);
                System.Threading.Thread.Sleep(playlist.Songs[i].Duration);
            }
            else if (color == "red")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(CutToDots(playlist.Songs[i].Title) + " " + playlist.Songs[i].Artist.Name +
                                                            " " + playlist.Songs[i].Duration +
                                                            " " + genre);
                System.Threading.Thread.Sleep(playlist.Songs[i].Duration);
            }
            else
            {
                Console.WriteLine(CutToDots(playlist.Songs[i].Title) + " " + playlist.Songs[i].Artist.Name +
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
    }
}
