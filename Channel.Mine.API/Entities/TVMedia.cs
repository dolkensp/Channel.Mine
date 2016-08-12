using System;
using System.IO;
using Channel.Mine.Framework.Abstraction;

namespace Channel.Mine.API.Entities
{
    public class TVMedia : BaseMedia
    {
        public TVMedia(String filePath)
        {
            this.File = new FileInfo(filePath);
        }
    }
}
