using System.Collections;
using System.IO;

namespace Channel.Mine.Framework.Abstraction
{
    public abstract class BaseMedia
    {
        public FileInfo File { get; set; }
        public Hashtable Context { get; set; }

        public BaseMedia()
        {
            this.Context = new Hashtable();
        }
    }
}
