using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer
{
    class Player
    {
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

        private bool playing;

        public bool Playing
        {
            get
            {
                return playing;
            }
        }

        bool IsLocked;
        public Song[] Songs;


        public void Play()
        {
            for (int i = 0; i < Songs.Length; i++)
            {
                Console.WriteLine(Songs[i].Title + " " + Songs[i].Artist.Name + " " + Songs[i].Duration);
                System.Threading.Thread.Sleep(Songs[i].Duration);
            }
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
    }
}
