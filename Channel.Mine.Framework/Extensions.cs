using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Channel.Mine.Framework
{
    public static class Extensions
    {
        #region String Extensions

        public static Int32? ToInt32(this String input)
        {
            Int32 temp;
            if (Int32.TryParse(input, out temp))
                return temp;
            return null;
        }

        public static Int32 ToInt32(this String input, Int32 @default) { return ToInt32(input) ?? @default; }

        #endregion
    }
}
