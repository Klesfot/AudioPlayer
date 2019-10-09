using System;
using System.Collections.Generic;

namespace AudioPlayer.lib.BL
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
