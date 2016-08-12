using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Channel.Mine.IMDB.Entities
{
    public enum MediaTypeEnum
    {
        Movie = 1,
        Movie_TV = 2,
        Movie_V = 3,
        TVShow = 4,
        VideoGame = 5,
    }

    public class Media
    {
        public Media()
        {
            this.ReleaseDates = new List<DateTime>();
            this.Names = new List<String>();
            this.Genres = new List<String>();
            this.Ratings = new List<String>();
        }

        public MediaTypeEnum MediaType { get; set; }
        public String Title { get; set; }
        public String Plot { get; set; }
        public String ReleaseYear { get; set; }
        public String EpisodeName { get; set; }
        public Int32? EpisodeNo { get; set; }
        public Int32? SeasonNo { get; set; }
        public List<DateTime> ReleaseDates { get; set; }
        public List<String> Names { get; set; }
        public List<String> Genres { get; set; }
        public List<String> Ratings { get; set; }

        public Double Score { get; set; }
        public String ID { get { return GetID(this.Title, this.ReleaseYear, this.EpisodeName, this.MediaType, this.SeasonNo, this.EpisodeNo); } }

        public override String ToString() { return this.ID; }

        public static String GetID(String title, String releaseYear, String episodeName, MediaTypeEnum mediaType, Int32? seasonNo, Int32? episodeNo)
        {
            return String.Format("[{3}] ({1}) {0} {2} {4}.{5}", title, releaseYear, episodeName, Enum.GetName(typeof(MediaTypeEnum), mediaType), seasonNo, episodeNo);
        }
    }
}
