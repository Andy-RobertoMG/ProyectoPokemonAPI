using ProyectoPokemon.Models;

namespace ProyectoPokemon.IService;

public interface IPokemonAPIService
{
    Task<PokemonListResponse> GetList(int limit = 0, int offset = 20);
    Task<PokemonModel> GetPokemonByUrl(string url);
    Task<PokemonModel> GetPokemonByName(string name);
    Task<PokemonSpecies> GetPokemonSpecie(string name);
    Task<PokemonListResponse> GetPokemonSpecieList();
}