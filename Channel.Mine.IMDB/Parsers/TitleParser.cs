using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Channel.Mine.IMDB.Parsers
{
    public class TitleParser : Abstraction.BaseFileParser<Collections.MediaCollection>
    {
        public TitleParser(params String[] fileNames) : base(fileNames) { }

        public override void DoAction(Collections.MediaCollection item)
        {
            throw new NotImplementedException();
        }
    }
}
