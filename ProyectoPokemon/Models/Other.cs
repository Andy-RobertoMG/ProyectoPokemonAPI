using System.Text.Json.Serialization;

namespace ProyectoPokemon.Models;

public class Other
{
    [JsonPropertyName("official-artwork")]
    public OfficialArtwork OfficialArtwork { get; set; } = new OfficialArtwork();
}