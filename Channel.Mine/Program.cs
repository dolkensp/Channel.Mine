using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Channel.Mine
{
    class Program
    {
        static void RebuildIndex()
        {
            Framework.Abstraction.DynamicPipeline<IMDB.Collections.MediaCollection> pipeline = null;

            #region Configure Pipeline

            pipeline &= new IMDB.Parsers.MovieParser(@"DataStore\movies.list");
            pipeline &= new IMDB.Parsers.GenreParser(@"DataStore\genres.list");
            // pipeline &= new IMDB.Parsers.RatingParser();
            // pipeline &= new IMDB.Parsers.TitleParser();
            // pipeline &= new IMDB.Parsers.PlotParser();
            // pipeline &= new IMDB.Parsers.ReleaseDateParser();
            pipeline &= new IMDB.Actions.IndexBuilder(@"DataStore");

            #endregion

            IMDB.Collections.MediaCollection mediaLibrary = new IMDB.Collections.MediaCollection();

            pipeline.Process(mediaLibrary);
        }

        static void ProcessMedia()
        {
            String rootPath = @"M:\TV Shows\";
            // String rootPath = @"C:\Users\peter.dolkens.DDRIT\Music\iTunes\iTunes Media\TV Shows\";

            Framework.Abstraction.DynamicPipeline<API.Entities.TVMedia> pipeline = null;
            API.Actions.TV.Collector collector = new API.Actions.TV.Collector();

            #region Configure Pipeline

            
            // pipeline &= new API.Filters.TV.FileSystemFilter(@"\Drop Dead Diva\");
            pipeline &= new API.Parsers.TV.FileSystemParser(rootPath);
            pipeline &= new API.Parsers.TV.IMDBParser(@"DataStore");
            pipeline &= collector;

            #endregion

            #region Process Items

            List<API.Entities.TVMedia> tvShows = new List<API.Entities.TVMedia>();

            new Thread(x => { 
#if DEBUG
            tvShows = (from filePath in File.ReadAllLines(@"c:\tvShows.txt")
                                                  let tvShow = new API.Entities.TVMedia(filePath)
                                                  where pipeline.Process(tvShow)
                                                  select tvShow).ToList<API.Entities.TVMedia>();
#else
            List<API.Entities.TVMedia> tvShows = (from filePath in Directory.GetFiles(rootPath, "*.m4v", SearchOption.AllDirectories)
                                                  let tvShow = new API.Entities.TVMedia(filePath)
                                                  where pipeline.Process(tvShow)
                                                  select tvShow).ToList<API.Entities.TVMedia>();
#endif
            }).Start();

            #endregion

            while (!Console.KeyAvailable && (tvShows.Count == 0))
            {
                Thread.Sleep(1000);
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(collector.Items.Count);
            }

            API.Entities.TVMedia[] collection = new API.Entities.TVMedia[collector.Items.Values.Count];
            collector.Items.Values.CopyTo(collection, 0);

            if (File.Exists(@"c:\hits.csv"))
                File.Delete(@"c:\hits.csv");

            foreach (var item in collection)
            {
                String row = String.Format("\"{0}\",\"{1}\"", item.File.DirectoryName, item.File.Name);
                foreach (IMDB.Entities.Media hit in item.Context[API.Parsers.TV.IMDBParser.IMDB_SEARCHHIT] as List<IMDB.Entities.Media>)
                    File.AppendAllText(@"c:\hits.csv", String.Format("{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"\r\n", row, hit.Score, hit.Title.Replace('"', ' '), hit.EpisodeName.Replace('"', ' '), hit.SeasonNo, hit.EpisodeNo));
            }

            var showsWithNoHits = (from show in tvShows
                                   let hits = show.Context[Channel.Mine.API.Parsers.TV.IMDBParser.IMDB_SEARCHHIT] as List<IMDB.Entities.Media>
                                   where hits.Count < 1
                                   select show).ToList();

            var showsWithHits = tvShows.Except(showsWithNoHits).ToList();

            Console.WriteLine("There are {0} items with no hits", showsWithNoHits.Count);

            Console.WriteLine("There are {0} items", tvShows.Count);
        }

        static void Main(string[] args)
        {
            // RebuildIndex();

            ProcessMedia();

            Console.ReadKey();
        }
    }
}
