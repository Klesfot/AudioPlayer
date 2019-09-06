using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer
{
    public class Song: PlayingItem
    {
        public int Duration;
        public string Title;
        public string Path;
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


        public Song(int duration = 0, string title = "Nothing Here", string path = "", Artist artist = null, int genre = 5)
        {
            Duration = duration;
            Title = title;
            Artist = artist;
            Genre = (int)genre;
            Path = path;
        }


        public Song()
        {

        }


        public void Deconstruct(out int duration, out string title, out Artist artist)
        {
            duration = Duration;
            title = Title;
            artist = Artist;
        }
    }
}
