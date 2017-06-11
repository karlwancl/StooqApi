using System;
using System.Runtime.Serialization;

namespace StooqApi
{
    [Flags]
    public enum SkipOption
    {
        [EnumMember(Value = "o_s")]
        Splits = 2 << 6,
        [EnumMember(Value = "o_d")]
        Dividends = 2 << 5,
        [EnumMember(Value = "o_p")]
        PreemptiveRights = 2 << 4,
        [EnumMember(Value = "o_n")]
        PrepurchaseRights = 2 << 3,
        [EnumMember(Value = "o_o")]
        PreaccesionRights = 2 << 2,
        [EnumMember(Value = "o_m")]
        Denominations = 2 << 1,
        [EnumMember(Value = "o_x")]
        Others = 2 << 0,
        None = 0
    }
}
