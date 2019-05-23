using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer
{
    class Playlist: IDisposable
    {
        private bool isDisposed = false;
        public string Path;
        public string Title;
        public List<Song> Songs = new List<Song>();

        ~Playlist()
        {
            if (isDisposed == false)
                Dispose();
        }


        public void Dispose()
        {
            Songs = null;
        }
    }
}
