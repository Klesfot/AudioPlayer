using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer
{
    class Player
    {   
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
                {
                    _volume = _maxVolume;
                }

                else if(value < 0)
                {
                    _volume = _minVolume;
                }

                else
                {
                    _volume = value;
                }
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

        bool IsLocked;
        

        public void Play()
        {   
            if (playing == true)
            {
                for (int i = 0; i < playlist.Songs.Count; i++)
                {
                    Console.WriteLine(playlist.Songs[i].Title + " " + playlist.Songs[i].Artist.Name +
                                                                            " " + playlist.Songs[i].Duration);
                    System.Threading.Thread.Sleep(playlist.Songs[i].Duration);
                }
            }
            else
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


        public Song Add(Song song1)
        {
            playlist.Songs.Add(song1);
            Console.WriteLine("Added song: " + " " + song1.Title + " " + song1.Artist.Name + " " + song1.Duration);
            return null;
        }


        public Song[] Add(Song song1, Song song2)
        {
            playlist.Songs.Add(song1);
            playlist.Songs.Add(song2);
            Console.WriteLine("Added song: " + " " + song1.Title + " " + song1.Artist.Name + " " + song1.Duration);
            Console.WriteLine("Added song: " + " " + song2.Title + " " + song2.Artist.Name + " " + song2.Duration);
            return null;
        }


        public Song[] Add(Song[] songs)
        {
            for (int i = 0; i < songs.Length; i++)
            {
                playlist.Songs.Add(songs[i]);
                Console.WriteLine("Added song: " + " " + songs[i].Title + " " + songs[i].Artist.Name + " " + songs[i].Duration);
            }
            return null;
        }
    }
}
