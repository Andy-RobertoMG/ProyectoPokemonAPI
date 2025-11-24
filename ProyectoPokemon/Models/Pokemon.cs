using System.Text.Json.Serialization;

namespace ProyectoPokemon.Models;

public class Pokemon
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}
