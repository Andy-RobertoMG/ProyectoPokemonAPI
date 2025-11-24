using System.Text.Json.Serialization;

namespace ProyectoPokemon.Models;

public class AbilityDetail
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}
