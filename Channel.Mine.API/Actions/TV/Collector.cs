using System;
using Channel.Mine.Framework.Abstraction;
using System.Collections.Generic;

namespace Channel.Mine.API.Actions.TV
{
    public class Collector : BaseAction<Entities.TVMedia>
    {
        public Dictionary<String, Entities.TVMedia> Items { get; private set; }

        public Collector() { this.Items = new Dictionary<String, Entities.TVMedia>(); }

        public override void DoAction(Entities.TVMedia item)
        {
            this.Items.Add(item.File.FullName, item);
        }
    }
}
