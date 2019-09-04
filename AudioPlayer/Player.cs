using System;
using System.Collections.Generic;
using ExtensionMethods;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;

namespace AudioPlayer
{
    class Player: GenericPlayer, IDisposable
    {
        public Player(ISkin skin)
        {
            this.currentSkin = skin;
        }

        public enum Genre : int
        {
            PsyTrance = 0,
            Electronic = 1,
            Hardcore = 2,
            DnB = 3,
            Drumstep = 4,
            NaN = 5
        };

        WaveOut wavPlayer = new WaveOut();
        public ISkin currentSkin = null;
        public Playlist playlist = new Playlist();

        public event Action<Song> SongStartedEvent;
        public event Action<Song> SongStoppedEvent;
        public event Action<List<Song>> SongListChangedEvent;
        public event Action<int> VolumeChangedEvent;
        public event Action<bool> PlayerStartedEvent;
        public event Action<bool> PlayerStoppedEvent;
        public event Action<bool> PlayerLockedEvent;
        public event Action<bool> PlayerUnlockedEvent;

        private readonly int _maxVolume = 100;
        private readonly int _minVolume = 0;
        private int _volume;
        private Song _playingSong = new Song();
        private List<Song> _playingPlaylist = new List<Song>();
        private bool isDisposed = false;
        private bool _isPlaying = true;
        private bool _isLocked;

        public int Volume
        {
            get
            {
                return _volume;
            }

            set
            {
                if (value > _maxVolume)
                    _volume = _maxVolume;

                else if (value < 0)
                    _volume = _minVolume;

                else
                    _volume = value;

                VolumeChangedEvent?.Invoke(_volume);
            }
        }

        public Song PlayingSong
        {
            get
            {
                return _playingSong;
            }

            set
            {
                _playingSong = value;
                SongStartedEvent?.Invoke(value);
            }
        }


        public List<Song> PlayingPlaylist
        {
            get
            {
                return _playingPlaylist;
            }

            set
            {
                _playingPlaylist = value;
                playlist.Songs = _playingPlaylist;
                SongListChangedEvent?.Invoke(value);
            }
        }


        public bool isPlaying
        {
            get
            {
                return _isPlaying;
            }

            private set
            {
                _isPlaying = value;
            }
        }

        public bool IsOnLoop;

        public bool IsLocked
        {
            get
            {
                return _isLocked;
            }

            set
            {
                _isLocked = value;
            }
        }

        public void Play(bool IsOnLoop = false)
        {
            if (isPlaying == true)
            {
                if (IsLocked == false)
                {
                    if (IsOnLoop)
                    {
                        //TODO: loop play, handle exception
                        Start();
                        for (int i = 0; i < PlayingPlaylist.Count; i++)
                        {
                           Task playSong = Task.Run(() =>
                           {
                               PlaySong(i);
                           });
                        }
                        Stop();
                    }

                    else
                    {
                        Start();
                        for (int i = 0; i < PlayingPlaylist.Count; i++)
                        {
                            Task playSong = Task.Run(() =>
                            {
                                PlaySong(i);
                            });
                        }
                        Stop();
                    }
                }
            }
        }

        
        public async void PlaySong(int i)
        {
            Mp3FileReader mp3Reader = new Mp3FileReader(PlayingPlaylist[i].Path);
            wavPlayer.Init(mp3Reader);
            wavPlayer.Play();
        }


        public void Start()
        {
            if (IsLocked == false)
            {
                isPlaying = true;
                PlayerStartedEvent?.Invoke(isPlaying);
            }
        }


        public void Stop()
        {
            if (IsLocked == false)
            {
                isPlaying = false;
                PlayerStoppedEvent?.Invoke(isPlaying);
            }
        }


        public void Lock()
        {
            IsLocked = true;
            PlayerLockedEvent?.Invoke(IsLocked);
        }


        public void Unlock()
        {
            IsLocked = false;
            PlayerUnlockedEvent?.Invoke(IsLocked);
        }


        public void VolumeUp()
        {
            Volume += 5;
        }


        public void VolumeDown()
        {
            Volume -= 5;
        }


        public void VolumeChange(int amount)
        {
            Volume = amount;
        }


        public void Load(string path)
        {
            List<Song> songs = new List<Song>();
            DirectoryInfo dir = new DirectoryInfo(path);

            foreach (var item in dir.EnumerateFiles())
            {
                Song song = new Song((int)item.Length / 1000,
                                          item.Name,
                                          item.FullName);
                song.Artist = new Artist("Test artist", "None", "Sweden");
                songs.Add(song);
            }

            playlist.Songs.AddRange(songs);
            PlayingPlaylist = playlist.Songs;
        }


        public void Clear()
        {
            PlayingPlaylist.Clear();
            SongListChangedEvent?.Invoke(PlayingPlaylist);
        }


        public List<Song> Shuffle()
        {
            this.PlayingPlaylist = this.PlayingPlaylist.Shuffle();
            return this.PlayingPlaylist;
        }


        public List<Song> SortByTitle()
        {
            this.PlayingPlaylist = this.PlayingPlaylist.SortByTitle();
            return this.PlayingPlaylist;
        }


        public List<Song> FilterByGenre(int input)
        {
            this.PlayingPlaylist = this.PlayingPlaylist.FilterByGenre(input);
            return this.PlayingPlaylist;
        }

        public string CutToDots(string data)
        {
            data = data.CutToDots();
            return data;
        }


        public void SaveCurrentPlaylist(string path = @"D:\Songs\SavedPlaylist.xml")
        {
            List<Song> songs = PlayingPlaylist;
            string result = "";
            string temp;

            XmlSerializer ser = new XmlSerializer(songs.GetType());

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    ser.Serialize(writer, songs);
                    temp = sww.ToString();
                    result += temp;
                }
                if (File.Exists(path))
                    File.Delete(path);
                else
                    File.Create(path);
                File.AppendAllText(path, result);
            }
        }

        public void LoadPlaylist(string path = @"D:\Songs\SavedPlaylist.xml")
        {
            List<Song> temp;
            XmlSerializer ser = new XmlSerializer(typeof(List<Song>));
            string xmlString = File.ReadAllText(path);

            using (TextReader reader = new StringReader(xmlString))
            {
                temp = (List<Song>)ser.Deserialize(reader);
            }

            playlist.Songs.AddRange(temp);
        }


        ~Player()
        {
            if (isDisposed == false)
                Dispose();
        }


        public void Dispose()
        {
            if (isDisposed == false)
            {
                isDisposed = true;
                wavPlayer.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}