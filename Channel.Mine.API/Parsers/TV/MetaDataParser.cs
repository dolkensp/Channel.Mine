using System;
using Channel.Mine.Framework.Abstraction;

namespace Channel.Mine.API.Parsers.TV
{
    public class MetaDataParser : BaseAction<Entities.TVMedia>
    {
        public const String MD_SERIES = "MD_Series";
        public const String MD_SEASON = "MD_Season";
        public const String MD_TITLE  = "MD_Title";

        public override void DoAction(Entities.TVMedia item)
        {
            // TODO: Load MetaData from file
        }
    }
}
