using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Channel.Mine.Framework;

namespace Channel.Mine.IMDB.Parsers
{
    public class MovieParser : Abstraction.BaseFileParser<Collections.MediaCollection>
    {
        public const String MEDIAPATTERN = @"^(.*? \(([0-9?]{4})/?[XVI]*\))(?: \((TV|VG|V)\)|)(?: \{(.*?)(?: ?\(#(\d+)\.(\d+)\)|)\}|)\t+";
        public const String SUSPENDED = @"{SUSPENDED}";
        
        private const String BOUNDARY_START = @"-----------------------------------------------------------------------------";
        private const String BOUNDARY_END = @"-----------------------------------------------------------------------------";
        private readonly String[] EXCLUDED = new String[] { @"MOVIES LIST", @"===========", @"" };

        public MovieParser(params String[] fileNames) : base(fileNames) { }

        public override void DoAction(Collections.MediaCollection item)
        {
            foreach(String file in this.FileNames)
            {
                this.ParserState = ParserStateEnum.Seeking;

                foreach (String line in File.ReadLines(file, Encoding.UTF7))
                {
                    try
                    {
                        switch (this.ParserState)
                        {
                            case ParserStateEnum.Seeking:
                                if (line == BOUNDARY_START)
                                    this.ParserState++;
                                break;
                            case ParserStateEnum.Parsing:
                                if (line == BOUNDARY_END)
                                    this.ParserState++;
                                else if (!EXCLUDED.Contains<String>(line))
                                {
                                    // Parse the line using regex
                                    Match parsed = Regex.Match(line, MovieParser.MEDIAPATTERN);

                                    // Default the Media Type to Movie
                                    Entities.MediaTypeEnum mediaType = Entities.MediaTypeEnum.Movie;

                                    // Ignore SUSPENDED entries
                                    if (parsed.Groups[4].Value == MovieParser.SUSPENDED)
                                        break;
                                    // Detect TV Shows
                                    else if (!String.IsNullOrWhiteSpace(parsed.Groups[4].Value) ||
                                             !String.IsNullOrWhiteSpace(parsed.Groups[5].Value) ||
                                             !String.IsNullOrWhiteSpace(parsed.Groups[6].Value))
                                        mediaType = Entities.MediaTypeEnum.TVShow;
                                    else
                                        // Determine Media Type of Non-TV Shows
                                        switch (parsed.Groups[3].Value)
                                        {
                                            case "TV":
                                                mediaType = Entities.MediaTypeEnum.Movie_TV;
                                                break;
                                            case "VG":
                                                mediaType = Entities.MediaTypeEnum.VideoGame;
                                                break;
                                            case "V":
                                                mediaType = Entities.MediaTypeEnum.Movie_V;
                                                break;
                                        }

                                    // Add the Media Item to the collection
                                    item.Add(new Entities.Media
                                    {
                                        Title = parsed.Groups[1].Value.Trim(),
                                        ReleaseYear = parsed.Groups[2].Value.Trim(),
                                        MediaType = mediaType,
                                        EpisodeName = parsed.Groups[4].Value.Trim('"', ' ', '\t'),
                                        SeasonNo = parsed.Groups[5].Value.ToInt32(),
                                        EpisodeNo = parsed.Groups[6].Value.ToInt32(),
                                    });
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}
