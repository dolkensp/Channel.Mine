using System;
using System.IO;
using Channel.Mine.Framework.Abstraction;
#region using Lucene.Net
using Analysis = Lucene.Net.Analysis;
using Documents = Lucene.Net.Documents;
using Index = Lucene.Net.Index;
using Standard = Lucene.Net.Analysis.Standard;
using Messages = Lucene.Net.Messages;
using QueryParsers = Lucene.Net.QueryParsers;
using Search = Lucene.Net.Search;
using Store = Lucene.Net.Store;
using Util = Lucene.Net.Util;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
#endregion

namespace Channel.Mine.API.Parsers.TV
{
    public class IMDBParser : BaseAction<Entities.TVMedia>
    {
        public const String IMDB_SEARCHHIT = "IMDB_SEARCHHIT";

        public const float IMDB_EPISODENAME_FULLBOOST = 1.8f;
        public const float IMDB_EPISODENAME_PARTBOOST = 0.1f;
        public const float IMDB_SERIESNAME_FULLBOOST = 2.0f;
        public const float IMDB_SERIESNAME_PARTBOOST = 0.2f;
        public const float IMDB_EPISODE_BOOST = 4.0f;
        public const float IMDB_SEASON_BOOST = 4.0f;

        public String IndexPath { get; private set; }
        public Int32 MaxHits { get; private set; }

        public IMDBParser(String indexPath, Int32 maxHits = 1)
        {
            this.IndexPath = indexPath;
            this.MaxHits = maxHits;
        }

        public override void DoAction(Entities.TVMedia item)
        {
            #region Initialize Variables

            Store.Directory directory = Store.FSDirectory.GetDirectory(this.IndexPath, false);
            Search.IndexSearcher searcher = new Search.IndexSearcher(directory, true);
            Standard.StandardAnalyzer analyzer = new Standard.StandardAnalyzer(Util.Version.LUCENE_29);

            Search.BooleanQuery multiTermQuery = new Search.BooleanQuery();
            Search.BooleanQuery typeQuery = new Search.BooleanQuery();
            Search.BooleanQuery titleQuery = new Search.BooleanQuery();
            Search.TermQuery termQuery = null;

            #endregion

            #region Title

            String field = "EpisodeName";
            String value = (item.Context[FileSystemParser.FS_TITLE] as String).ToLowerInvariant();
            float boostMultiplier = 1.0f;
            
            if (!String.IsNullOrWhiteSpace(value))
            {
                termQuery = new Search.TermQuery(new Index.Term(field, value));
                termQuery.SetBoost(IMDBParser.IMDB_EPISODENAME_FULLBOOST * boostMultiplier);
                titleQuery.Add(termQuery, Search.BooleanClause.Occur.SHOULD);
                
                foreach (String term in value.Split(' ', '.', '_', '-', '&'))
                {
                    termQuery = new Search.TermQuery(new Index.Term(field, term));
                    termQuery.SetBoost(IMDBParser.IMDB_EPISODENAME_PARTBOOST * boostMultiplier);
                    titleQuery.Add(termQuery, Search.BooleanClause.Occur.SHOULD);
                }
            }

            field = "Title";
            value = (item.Context[FileSystemParser.FS_SERIES] as String).ToLowerInvariant();
            boostMultiplier = 3.0f;

            if (String.IsNullOrWhiteSpace(value))
            {
                value = (item.Context[FileSystemParser.FS_TITLE] as String).ToLowerInvariant();
                boostMultiplier = 1.3f;
            }

            if (!String.IsNullOrWhiteSpace(value))
            {
                termQuery = new Search.TermQuery(new Index.Term(field, value));
                termQuery.SetBoost(IMDBParser.IMDB_SERIESNAME_FULLBOOST * boostMultiplier);
                titleQuery.Add(termQuery, Search.BooleanClause.Occur.SHOULD);

                foreach (String term in value.Split(' ', '.', '_', '-'))
                {
                    termQuery = new Search.TermQuery(new Index.Term(field, term));
                    termQuery.SetBoost(IMDBParser.IMDB_SERIESNAME_PARTBOOST * boostMultiplier);
                    titleQuery.Add(termQuery, Search.BooleanClause.Occur.SHOULD);
                }
            }

            titleQuery.SetMinimumNumberShouldMatch(1);
            multiTermQuery.Add(titleQuery, Search.BooleanClause.Occur.MUST);

            #endregion

            #region MediaType

            typeQuery.Add(new Search.TermQuery(new Index.Term("MediaType", "TVShow")), Search.BooleanClause.Occur.SHOULD);
            typeQuery.Add(new Search.TermQuery(new Index.Term("MediaType", "Movie_TV")), Search.BooleanClause.Occur.SHOULD);

            typeQuery.SetMinimumNumberShouldMatch(1);
            multiTermQuery.Add(typeQuery, Search.BooleanClause.Occur.MUST);

            #endregion

            #region EpisodeNo

            field = "EpisodeNo";
            value = (item.Context[FileSystemParser.FS_EPISODE] as String);
            boostMultiplier = 2.0f;

            if (!String.IsNullOrWhiteSpace(value))
            {
                termQuery = new Search.TermQuery(new Index.Term(field, value.TrimStart('0')));
                termQuery.SetBoost(IMDBParser.IMDB_EPISODE_BOOST * boostMultiplier);
                multiTermQuery.Add(termQuery, Search.BooleanClause.Occur.MUST);
            }

            #endregion

            #region SeasonNo

            field = "SeasonNo";
            value = (item.Context[FileSystemParser.FS_SEASON] as String);
            boostMultiplier = 2.0f;

            if (!String.IsNullOrWhiteSpace(value))
            {
                termQuery = new Search.TermQuery(new Index.Term(field, value.TrimStart('0')));
                termQuery.SetBoost(IMDBParser.IMDB_SEASON_BOOST * boostMultiplier);
                multiTermQuery.Add(termQuery, Search.BooleanClause.Occur.MUST);
            }

            #endregion

            #region Search

            Search.TopDocs topDocs = searcher.Search(multiTermQuery, this.MaxHits);

            List<IMDB.Entities.Media> castHits = new List<IMDB.Entities.Media>();

            foreach(Search.ScoreDoc topDoc in topDocs.scoreDocs)
            {
                IMDB.Entities.Media hit = JsonConvert.DeserializeObject<IMDB.Entities.Media>(searcher.Doc(topDoc.doc).GetField("Object").StringValue());

                hit.Score = topDoc.score;

                if ((hit.MediaType == IMDB.Entities.MediaTypeEnum.Movie_TV) || (hit.MediaType == IMDB.Entities.MediaTypeEnum.TVShow))
                    castHits.Add(hit);
            }

            item.Context[IMDBParser.IMDB_SEARCHHIT] = castHits;

            #endregion

            #region Dispose

            analyzer.Close();
            searcher.Close();
            directory.Close();

            #endregion
        }
    }
}