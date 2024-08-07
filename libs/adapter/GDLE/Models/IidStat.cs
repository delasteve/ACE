using System.Text.Json.Serialization;
using Lifestoned.DataModel.Shared;

namespace ACE.Adapter.GDLE.Models;

public class IidStat
{
    [JsonPropertyName("key")]
    public int Key { get; set; }

    [JsonPropertyName("value")]
    public int Value { get; set; }

    [JsonIgnore]
    public string PropertyIdBinder => ((IidPropertyId)Key).GetName();

    [JsonIgnore]
    public bool Deleted { get; set; }
}
