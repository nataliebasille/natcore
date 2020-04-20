using System;
using System.Collections.Generic;
using System.Text;

namespace Natcore.Core.Exceptions
{
    public static partial class ThrowIf
    {
        public static class Argument
        {
            public static void IsNull(object argumentValue, string argumentName)
            {
                if (argumentValue == null)
                    throw new ArgumentNullException(argumentName);
            }
        }
    }
}
