namespace ProyectoPokemon.Models;
using System.Text.Json.Serialization;

public class PokemonListResponse
{
    [JsonPropertyName("count")]
    public int Count { get; set; } = 0;

    [JsonPropertyName("next")]
    public string? Next { get; set; } 

    [JsonPropertyName("previous")]
    public string? Previous { get; set; }

    [JsonPropertyName("results")]
    public List<Pokemon> Results { get; set; } = new();
}
