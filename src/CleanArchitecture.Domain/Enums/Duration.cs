using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CleanArchitecture.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Duration
    {
        Monthly,
        QuarterYear,
        HalfYear,
        Year
    }
}