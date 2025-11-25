using System.Text.Json.Serialization;

namespace ProyectoPokemon.Models;

public class PokemonModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; } = 0;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("base_experience")]
    public int? BaseExperience { get; set;} = 0;
    
    [JsonPropertyName("height")]
    public int? Height {get; set;} = 0;

    [JsonPropertyName("weight")]
    public int? Weight { get; set; } = 0;

    [JsonPropertyName("sprites")]
    public Sprites Sprites { get; set; } = new Sprites();

    [JsonPropertyName("species")]
    public Pokemon Species { get; set; } = new();
}




