using System.Text.Json.Serialization;

namespace ProyectoPokemon.Models;

public class PokemonSpecies
{
    [JsonPropertyName("varieties")]
    public List<Varieties> Varieties { get; set; } = new();
}

public class Varieties
{
    [JsonPropertyName("is_default")]
    public bool IsDefault { get; set; }

    [JsonPropertyName("pokemon")]
    public Pokemon Pokemon { get; set; } = new();
}