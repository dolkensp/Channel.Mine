using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
#endregion

namespace Channel.Mine.IMDB.Actions
{
    public class IndexBuilder : Framework.Abstraction.BaseAction<Collections.MediaCollection>
    {
        public String IndexPath { get; private set; }

        public IndexBuilder(String indexPath)
        {
            this.IndexPath = indexPath;
        }
        
        public override void DoAction(Collections.MediaCollection mediaCollection)
        {
            Store.Directory directory = Store.FSDirectory.GetDirectory(this.IndexPath, true);
            Standard.StandardAnalyzer analyzer = new Standard.StandardAnalyzer(Util.Version.LUCENE_29);
            Index.IndexWriter writer = new Index.IndexWriter(directory, analyzer, true, Index.IndexWriter.MaxFieldLength.UNLIMITED);

            Documents.Document document = null;
            Documents.Field newField = null;

            foreach (Entities.Media mediaItem in mediaCollection)
            {
                document = new Documents.Document();

                document.Add(new Documents.Field("MediaType", Enum.GetName(typeof(Entities.MediaTypeEnum), mediaItem.MediaType), Documents.Field.Store.YES, Documents.Field.Index.NOT_ANALYZED, Documents.Field.TermVector.YES));
                document.Add(new Documents.Field("Title", mediaItem.Title, Documents.Field.Store.YES, Documents.Field.Index.ANALYZED, Documents.Field.TermVector.YES));
                document.Add(new Documents.Field("EpisodeName", mediaItem.EpisodeName, Documents.Field.Store.YES, Documents.Field.Index.ANALYZED, Documents.Field.TermVector.YES));
                document.Add(new Documents.Field("EpisodeNo", mediaItem.EpisodeNo.ToString(), Documents.Field.Store.YES, Documents.Field.Index.NOT_ANALYZED, Documents.Field.TermVector.YES));
                document.Add(new Documents.Field("SeasonNo", mediaItem.SeasonNo.ToString(), Documents.Field.Store.YES, Documents.Field.Index.NOT_ANALYZED, Documents.Field.TermVector.YES));
                document.Add(new Documents.Field("Genre", String.Join(", ", mediaItem.Genres), Documents.Field.Store.YES, Documents.Field.Index.ANALYZED, Documents.Field.TermVector.YES));
                document.Add(new Documents.Field("Names", String.Join(", ", mediaItem.Names), Documents.Field.Store.YES, Documents.Field.Index.ANALYZED, Documents.Field.TermVector.YES));
                document.Add(new Documents.Field("ReleaseYear", mediaItem.ReleaseYear, Documents.Field.Store.YES, Documents.Field.Index.NOT_ANALYZED, Documents.Field.TermVector.YES));
                
                document.Add(new Documents.Field("Object", JsonConvert.SerializeObject(mediaItem), Documents.Field.Store.YES, Documents.Field.Index.NOT_ANALYZED, Documents.Field.TermVector.NO));
                
                writer.AddDocument(document);
            }

            writer.Commit();
            writer.Optimize();
            writer.Close();
            analyzer.Close();
            directory.Close();
        }
    }
}