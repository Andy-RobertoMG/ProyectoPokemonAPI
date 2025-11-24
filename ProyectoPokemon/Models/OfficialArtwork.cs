using System.Text.Json.Serialization;

namespace ProyectoPokemon.Models;

public class OfficialArtwork
{
    [JsonPropertyName("front_default")]
    public string FrontDefault { get; set; } = string.Empty;
}
