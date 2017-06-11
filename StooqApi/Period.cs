using System;
using System.Runtime.Serialization;

namespace StooqApi
{
    public enum Period
    {
        [EnumMember(Value = "d")]
        Daily,
        [EnumMember(Value = "w")]
        Weekly,
        [EnumMember(Value = "m")]
        Monthly,
        [EnumMember(Value = "q")]
        Quarterly,
        [EnumMember(Value = "y")]
        Yearly
    }
}
