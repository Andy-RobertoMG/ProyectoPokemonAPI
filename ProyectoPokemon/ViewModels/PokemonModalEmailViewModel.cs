using System.ComponentModel.DataAnnotations;

namespace ProyectoPokemon.ViewModels;

public class PokemonModalEmailViewModel
{
    public int ActualPage { get; set; } = 0;
    public string SearchPokemon { get; set; } = string.Empty;
    public string PokemonSpecieName { get; set; } = string.Empty;
    [Required(ErrorMessage = "El correo es obligatorio")]
    public string UserRemitentEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}
