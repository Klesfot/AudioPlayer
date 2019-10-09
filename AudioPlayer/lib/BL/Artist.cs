namespace AudioPlayer.lib.BL
{
    public class Artist
    {
        public string Name;
        public string Nickname;
        public string Country;
        public Artist(string Name = "Unknown artist",
                      string Nickname = "N/A",
                      string Country = "-"){
            this.Name = Name;
        }

        public Artist()
        {

        }
    }
}
