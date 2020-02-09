using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Natcore.EntityFramework.Converters
{
    public class EnumToStringConverter<TEnum> : ValueConverter<TEnum, string>
        where TEnum : Enum
    {
        public EnumToStringConverter()
            : base(e => e.ToString(), v => (TEnum)Enum.Parse(typeof(TEnum), v))
        { }
    }

    public static partial class ValueConverters
    {
        public static ValueConverter<TEnum, string> EnumToString<TEnum>()
            where TEnum: Enum
            => new EnumToStringConverter<TEnum>();
    }
}
