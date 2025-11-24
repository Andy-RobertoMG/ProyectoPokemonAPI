using ProyectoPokemon.IService;
using ProyectoPokemon.Models;
using System.Xml.Linq;

namespace ProyectoPokemon.Service;

public class PokemonAPIService : IPokemonAPIService
{
    private string url = "https://pokeapi.co/api/v2/";
    public PokemonAPIService()
    {

    }
    public async Task<PokemonListResponse> GetList(int limit = 0, int offset =0)
    {

        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"{url}/pokemon?limit={limit}&offset={offset}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PokemonListResponse>()??new();
        }
        else
        {
            string resultadoMensajeError = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener la lista de Pokémon.Razon: {response.ReasonPhrase}. {resultadoMensajeError}");
        }
    }
    public async Task<PokemonModel> GetPokemonByUrl(string url)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"{url}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PokemonModel>() ?? new();
        }
        else
        {
            string resultadoMensajeError = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener el Pokémon por url.Razon: {response.ReasonPhrase}. {resultadoMensajeError}");
        }
    }
    public async Task<PokemonModel> GetPokemonByName(string name)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"{url}/pokemon/{name}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PokemonModel>() ?? new();
        }
        else
        {
            string resultadoMensajeError = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener el Pokémon por nombre. Razon: {response.ReasonPhrase}. {resultadoMensajeError}");
        }
    }
    public async Task<PokemonSpecies> GetPokemonSpecie(string name)
    {
        //Obtener la especie de Pokémon por nombre
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"{url}/pokemon-species/{name}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PokemonSpecies>() ?? new();
        }
        else
        {
            string resultadoMensajeError = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener la especie de Pokémon. Razon: {response.ReasonPhrase}. {resultadoMensajeError}");
        }
    }
    public async Task<PokemonListResponse> GetPokemonSpecieList()
    {
        //Obtener lista de especies de Pokémon
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"{url}/pokemon-species");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PokemonListResponse>() ?? new();
        }
        else
        {
            string resultadoMensajeError = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener el listado de especies de pokemon.Razon: {response.ReasonPhrase}. {resultadoMensajeError}");
        }
    }
}
