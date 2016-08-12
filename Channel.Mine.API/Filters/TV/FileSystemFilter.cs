using System;
using Channel.Mine.API.Abstraction;

namespace Channel.Mine.API.Filters.TV
{
    public class FileSystemFilter : BaseFileSystemFilter<Entities.TVMedia>
    {
        public FileSystemFilter(String query) : base(query) { }
    }
}