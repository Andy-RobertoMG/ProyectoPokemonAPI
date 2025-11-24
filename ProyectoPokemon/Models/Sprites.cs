using System.Text.Json.Serialization;

namespace ProyectoPokemon.Models;

public class Sprites
{
    [JsonPropertyName("other")]
    public Other Other { get; set; } = new Other();
}