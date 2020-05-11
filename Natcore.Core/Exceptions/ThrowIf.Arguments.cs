using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Natcore.Core.Exceptions
{
    public static partial class ThrowIf
    {
        public static class Argument
        {
            public static void IsNull(object argumentValue, string argumentName, string message = null)
            {
                if (argumentValue == null)
                {
                    if (message == null)
                        throw new ArgumentNullException(argumentName);
                    else
                        throw new ArgumentNullException(argumentName, message);
                }
            }

            public static void IsNullOrEmpty(IEnumerable<object> argumentValue, string argumentName, string message = null)
            {
                if (argumentValue == null)
                    throw new ArgumentException(message ?? defaultNullMessage(), argumentName);

                if (!argumentValue.Any())
                    throw new ArgumentException(message ?? defaultEmptyMessage(), argumentName);

                string defaultNullMessage()
                {
                    return $"IEnumerable {argumentName} cannot be null";
                }

                string defaultEmptyMessage()
                {
                    return $"IEnumerable {argumentName} cannot be empty";
                }
            }
        }
    }
}
