using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Channel.Mine.IMDB.Parsers
{
    public class PlotParser : Abstraction.BaseFileParser<Collections.MediaCollection>
    {
        public PlotParser(params String[] fileNames) : base(fileNames) { }

        public override void DoAction(Collections.MediaCollection item)
        {
            throw new NotImplementedException();
        }
    }
}
