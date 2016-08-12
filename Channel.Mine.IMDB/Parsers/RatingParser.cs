using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Channel.Mine.IMDB.Parsers
{
    public class RatingParser : Abstraction.BaseFileParser<Collections.MediaCollection>
    {
        public RatingParser(params String[] fileNames) : base(fileNames) { }

        public override void DoAction(Collections.MediaCollection item)
        {
            throw new NotImplementedException();
        }
    }
}
