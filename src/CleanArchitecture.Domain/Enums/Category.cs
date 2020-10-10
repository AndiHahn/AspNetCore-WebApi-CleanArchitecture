using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CleanArchitecture.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Category
    {
        Food,
        Flat,
        Clothes,
        Education,
        Pleasure,
        Sport,
        Travelling,
        Car,
        HygieneAndHealth,
        Gift
    }
}
