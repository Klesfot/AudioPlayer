using System.Linq;
using System.Collections.Generic;
using AudioPlayer;

namespace ExtensionMethods
{
    public class Helper
    {
        public static List<Song> SortByTitle(this List<Song> Songs)
        {
            var tempNameList = new List<String>();
            List<Song> newSongList = Songs.OrderBy(o => o.Title).ToList();

            return newSongList;
        }


        public static List<Song> SortByGenre(this List<Song> Songs)
        {
            var tempNameList = new List<String>();
            List<Song> newSongList = Songs.OrderBy(o => (int)o.Genre).ToList();

            return newSongList;
        }


        static Random rand = new Random();
        public static List<Song> Shuffle(this List<Song> Songs)
        {
            var newSongs = new List<Song>();

            for (int i = Songs.Count - 1; i >= 0; i--)
            {
                var r = rand.Next(i + 1);
                var temp = Songs[i];
                Songs[i] = Songs[r];
                Songs[r] = temp;
                newSongs.Add(Songs[i]);
            }
            return newSongs;
        }
    }
}
