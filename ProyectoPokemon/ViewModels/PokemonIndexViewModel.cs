using ProyectoPokemon.Models;

namespace ProyectoPokemon.ViewModels;
public class PokemonIndexViewModel
{
    public Pagination<PokemonModel> Pagination { get; set; } = new(new(),0,1,0);
    public PokemonListResponse PokemonSpeciesListResponse { get; set; } = new();
    public string SearchPokemon { get; set; } = string.Empty;
    public string PokemonSpecieName { get; set; } = "";
  


}
