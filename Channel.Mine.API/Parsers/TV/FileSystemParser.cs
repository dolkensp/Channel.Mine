using System;
using System.IO;
using Channel.Mine.Framework.Abstraction;
using System.Text.RegularExpressions;

namespace Channel.Mine.API.Parsers.TV
{
    public class FileSystemParser : BaseAction<Entities.TVMedia>
    {
        public const String FS_SERIES = "FS_SERIES";
        public const String FS_SEASON = "FS_SEASON";
        public const String FS_TITLE  = "FS_TITLE";
        public const String FS_EPISODE = "FS_EPISODE";

        public String RootPath { get; set; }

        public FileSystemParser(String rootPath)
        {
            this.RootPath = rootPath;
        }

        public override void DoAction(Entities.TVMedia item)
        {
            String[] parts = Path.ChangeExtension(item.File.FullName, null).Substring(this.RootPath.Length).Split('\\', '/');

            switch (parts.Length)
            {
                case 1:
                    item.Context[FS_SERIES] = String.Empty;
                    item.Context[FS_SEASON] = String.Empty;
                    item.Context[FS_TITLE]  = parts[0];
                    break;
                case 2:
                    item.Context[FS_SERIES] = parts[0];
                    item.Context[FS_SEASON] = String.Empty;
                    item.Context[FS_TITLE]  = parts[1];
                    break;
                case 3:
                    item.Context[FS_SERIES] = parts[0];
                    item.Context[FS_SEASON] = parts[1].Replace("Season ", "");
                    item.Context[FS_TITLE]  = parts[2];
                    break;
            }

            Match episode;

            if ((episode = Regex.Match(item.Context[FS_TITLE] as String, @"S(\d+) ?E(\d+)", RegexOptions.IgnoreCase)).Success ||
                (episode = Regex.Match(item.Context[FS_TITLE] as String, @"(\d+)x(\d+)", RegexOptions.IgnoreCase)).Success ||
                (episode = Regex.Match(item.Context[FS_TITLE] as String, @"\[(\d+) (\d+)\]", RegexOptions.IgnoreCase)).Success ||
                (episode = Regex.Match(item.Context[FS_TITLE] as String, @"(\d+).(\d+)", RegexOptions.IgnoreCase)).Success) 
            {
                item.Context[FS_SEASON] = episode.Groups[1].Value;
                item.Context[FS_EPISODE] = episode.Groups[2].Value;
            }
            else if ((episode = Regex.Match(item.Context[FS_TITLE] as String, @"^(\d+)(?: |-)(.*?)$", RegexOptions.IgnoreCase)).Success)
            {
                item.Context[FS_EPISODE] = episode.Groups[1].Value;
                item.Context[FS_TITLE] = episode.Groups[2].Value;
            }
        }
    }
}
