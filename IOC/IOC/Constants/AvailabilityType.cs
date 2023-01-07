using System.Runtime.Serialization;

namespace IOC.Constants
{
    public enum AvailabilityType
    {
        [EnumMember(Value = "Free")]
        Free,

        [EnumMember(Value = "TeaTime")]
        TeaTime,

        [EnumMember(Value = "Random")]
        Random
    }
}
