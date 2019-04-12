using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer
{
    class Player
    {
        private int _maxVolume;
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
                    _volume = value;
                }

                else
                {
                    _volume = value;
                }
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
    }
}
