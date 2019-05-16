using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer
{
    public class Song
    {
        public int Duration;
        public string Title;
        string Path;
        string Lyrics;
        public bool? IsLiked = null;
        public int Genre;

        public Artist Artist;
        Album Album;

        public void Like()
        {
            IsLiked = true;
        }


        public void Dislike()
        {
            IsLiked = false;
        }


        public Song(int duration = 0, string title = "Nothing Here", Artist artist = null, int genre = 5)
        {
            this.Duration = duration;
            this.Title = title;
            this.Artist = artist;
            this.Genre = (int)genre;
        }

        public void Deconstruct(out int duration, out string title, out Artist artist)
        {
            duration = Duration;
            title = Title;
            artist = Artist;
        }
    }
}
