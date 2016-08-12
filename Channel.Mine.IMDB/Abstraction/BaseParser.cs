using System;

namespace Channel.Mine.IMDB.Abstraction
{
    public abstract class BaseFileParser<T> : Framework.Abstraction.BaseAction<T>
    {
        internal enum ParserStateEnum
        {
            Seeking = 0,
            Parsing = 1,
            Closing = 2
        }

        internal String[] FileNames { get; private set; }

        internal ParserStateEnum ParserState { get; set; }

        public BaseFileParser(params String[] fileNames)
        {
            this.ParserState = ParserStateEnum.Seeking;
            this.FileNames = fileNames;
        }
    }
}
