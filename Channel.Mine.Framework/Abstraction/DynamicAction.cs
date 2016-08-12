using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Channel.Mine.Framework.Abstraction
{
    public delegate void ActionDelegate<T>(T item);

    public sealed class DynamicAction<T> : BaseAction<T>
    {
        public ActionDelegate<T> Action { get; private set; }

        public DynamicAction(ActionDelegate<T> action) { this.Action = action; }

        public override void DoAction(T item) { this.Action(item);  }
    }
}
