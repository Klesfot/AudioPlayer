using System;
using System.Collections.Generic;
using ExtensionMethods;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Timers;
using NAudio.Wave;

namespace AudioPlayer
{
    class Player : GenericPlayer, IDisposable
    {
        public Player(ISkin skin)
        {
            currentSkin = skin;
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


        public WaveOut currentPlaybackDevice = new WaveOut();
        public ISkin currentSkin = null;
        public Playlist playlist = new Playlist();

        public event Action<Song> SongStartedEvent;
        public event Action<Song> SongStoppedEvent;
        public event Action<List<Song>> SongListChangedEvent;
        public event Action<float> VolumeChangedEvent;
        public event Action<bool> PlayerStartedEvent;
        public event Action<bool> PlayerStoppedEvent;
        public event Action<bool> PlayerLockedEvent;
        public event Action<bool> PlayerUnlockedEvent;
        public event Action<bool> PlaybackAbortedEvent;


        private readonly float _maxVolume = 1;
        private readonly float _minVolume = 0;
        private Song _playingSong = new Song();
        private List<Song> _playingPlaylist = new List<Song>();
        private bool isDisposed = false;
        private float _volume = 0;
        private bool isOnLoop = false;
        private bool _isPlaying = true;
        private bool _isLocked;
        private bool _isAborted;


        //TODO: save volume to file
        public float Volume
        {
            get
            {
                return _volume;
            }

            set
            {
                if (value > _maxVolume)
                    currentPlaybackDevice.Volume = _maxVolume;

                else if (value < 0)
                    currentPlaybackDevice.Volume = _minVolume;

                else
                    currentPlaybackDevice.Volume = value;

                VolumeChangedEvent?.Invoke(currentPlaybackDevice.Volume);
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


        public bool IsPlaying
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


        public bool IsOnLoop
        {
            get
            {
                return isOnLoop;
            }
            set
            {
                isOnLoop = value;
            }
        }


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


        public bool IsAborted
        {
            get
            {
                return _isAborted;
            }

            set
            {
                if (value)
                {
                    PlaybackAbortedEvent?.Invoke(true);
                }

                else if (!value)
                {
                    PlayerStartedEvent?.Invoke(true);
                }
                _isAborted = value;
            }
        }


        public async void Play(bool IsOnLoop = false)
        {
            if (IsPlaying == true)
            {
                if (IsLocked == false)
                {
                    if (IsOnLoop)
                    {
                        int index = 0;

                        for (int songIndex = 0; songIndex < PlayingPlaylist.Count; songIndex++)
                        {
                            if (PlayingPlaylist[songIndex].Path == PlayingSong.Path)
                                index = songIndex;
                        }
                        while (IsOnLoop)
                        {
                            await PlayMP3Song(index).ConfigureAwait(false);
                        }
                    }

                    else
                    {
                        for (int songIndex = 0; songIndex < PlayingPlaylist.Count; songIndex++)
                        {
                            await PlayMP3Song(songIndex).ConfigureAwait(false);
                        }
                    }
                }
            }
        }


        public Task PlayMP3Song(int songIndex)
        {
            return Task.Run(() =>
            {
                using (WaveOut waveOut = new WaveOut())
                {
                    currentPlaybackDevice = waveOut;
                    SchedulePlaybackStateCheck(currentPlaybackDevice);

                    Start();
                    PlayingSong = PlayingPlaylist[songIndex];
                    using (Mp3FileReader mp3Reader = new Mp3FileReader(PlayingPlaylist[songIndex].Path))
                    {

                        currentPlaybackDevice.Init(mp3Reader);
                        currentPlaybackDevice.Play();

                        //That's a way of stopping control flow, if deleted waveout device is overflown with different file reader inputs
                        //Possibly try event waveOutDevice.PlaybackStopped?
                        while (currentPlaybackDevice.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(10);
                        }

                        Stop(currentPlaybackDevice);
                        currentPlaybackDevice.Dispose();
                    }
                }
            });
        }


        public Task SchedulePlaybackStateCheck(WaveOut playbackDevice)
        {
            return Task.Run(() =>
            {
                PlaybackAbortedEvent += (bool isAborted) =>
                {
                    playbackDevice.Stop();
                };
            });
        }


        public void Start()
        {
            if (IsLocked == false)
            {
                IsPlaying = true;
                PlayerStartedEvent?.Invoke(IsPlaying);
            }
        }


        public void Stop(WaveOut waveOutDevice)
        {
            if (IsLocked == false)
            {
                IsPlaying = false;
                PlayingSong = new Song();
                waveOutDevice.Stop();
                PlayerStoppedEvent?.Invoke(IsPlaying);
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


        public void VolumeChange(float amount)
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
            PlayingPlaylist = PlayingPlaylist.Shuffle();
            return PlayingPlaylist;
        }


        public List<Song> SortByTitle()
        {
            PlayingPlaylist = PlayingPlaylist.SortByTitle();
            return PlayingPlaylist;
        }


        public List<Song> FilterByGenre(int input)
        {
            PlayingPlaylist = PlayingPlaylist.FilterByGenre(input);
            return PlayingPlaylist;
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


        public void Abort()
        {
            IsAborted = true;
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
                currentPlaybackDevice.Dispose();
            }
        }
    }
}