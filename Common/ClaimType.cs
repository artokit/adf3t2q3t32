using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Common;

[JsonConverter(typeof(StringEnumConverter))]
public enum ClaimType
{
    Id
}
