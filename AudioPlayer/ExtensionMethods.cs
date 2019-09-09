using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using AudioPlayer;
using NAudio.Wave;

namespace ExtensionMethods
{
    public static class Extensions
    {
        static Random rand = new Random();
        public static List<Song> Shuffle(this List<Song> songs)
        {
            var newSongs = new List<Song>();

            for (int i = songs.Count - 1; i >= 0; i--)
            {
                var r = rand.Next(i + 1);
                var temp = songs[i];
                songs[i] = songs[r];
                songs[r] = temp;
                newSongs.Add(songs[i]);
            }
            return newSongs;
        }


        public static List<Song> SortByTitle(this List<Song> songs)
        {
            var tempNameList = new List<String>();
            List<Song> newSongList = songs.OrderBy(o => o.Title).ToList();

            return newSongList;
        }


        public static List<Song> FilterByGenre(this List<Song> songs, int genre)
        {
            var newSongsList = new List<Song>();

            for (int i = 0; i < songs.Count; i++)
            {
                if (songs[i].Genre == genre)
                    newSongsList.Add(songs[i]);
            }

            return newSongsList;
        }


        public static List<Song> ClearAll(this List<Song> songs)
        {
            songs.Clear();
            return songs;
        }

        
        public static String CutToDots(this String data)
        {
            if (data.Length > 14)
            {
                data = data.Remove(14);
                data = data.Insert(14, "...");
            }

            return data;
        }
    }
}