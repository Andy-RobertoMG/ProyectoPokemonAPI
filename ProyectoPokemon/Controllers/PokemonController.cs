using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using ProyectoPokemon.IService;
using ProyectoPokemon.Models;
using ProyectoPokemon.ViewModels;
using System.Diagnostics;
using System.IO;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
namespace ProyectoPokemon.Controllers;

public class PokemonController : Controller
{
    private readonly ILogger<PokemonController> _logger;
    private readonly IPokemonAPIService _pokemonService;
    private readonly IConfiguration _configuration;
    public PokemonController(ILogger<PokemonController> logger, IPokemonAPIService pokemonService,IConfiguration configuration)
    {
        _logger = logger;
        _pokemonService = pokemonService;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index(int pageActual = 1, string searchPokemon = "",string pokemonSpecieName="")
    {
        try
        {
            var result= await LoadData(pageActual, searchPokemon, pokemonSpecieName);
            return View(result);
        }
        catch(Exception ex)
        {
            TempData["Error"] = "Error al cargar la informacion " + ex.Message;
            return View(new PokemonIndexViewModel
            {
                Pagination = new Pagination<PokemonModel>(new List<PokemonModel>(), 0, 1, 10),
                PokemonSpecieName = pokemonSpecieName,
                SearchPokemon = searchPokemon
            }); 
        }

    }

    [HttpGet]
    public async Task<IActionResult> _InfoPokemon(int id)
    {
        return PartialView("_PokemonInfoDetails", "");
    }
    [HttpGet]
    public async Task<IActionResult> ExportToExcel(int pageActual = 1, string searchPokemon = "", string pokemonSpecieName = "")
    {
        try
        {
            
            var result = await LoadData(pageActual, searchPokemon, pokemonSpecieName);
            //Nombre del archivo
            string nombreArchivo = $"Lista_Pokemons_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            using MemoryStream file =GetFileExcel(result.Pagination.Items);
            return File(
                file.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                nombreArchivo
            );
        }
        catch(Exception ex)
        {
            TempData["Error"] = "Error al exportar la informacion " + ex.Message;
            return RedirectToAction("Index");
        }
    }
    public MemoryStream GetFileExcel(List<PokemonModel> list)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Datos");

            // Encabezados
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Nombre";
            worksheet.Cell(1, 3).Value = "Imagen";

            // Se llenan los datos del pokemon
            int row = 2;
            foreach (var item in list)
            {
                worksheet.Cell(row, 1).Value = item.Id;
                worksheet.Cell(row, 2).Value = item.Name;
                worksheet.Cell(row, 3).Value = item.Sprites.Other.OfficialArtwork.FrontDefault;
                row++;
            }
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }
    }
    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<PokemonIndexViewModel> LoadData(int pageActual = 1, string searchPokemon = "", string pokemonSpecieName = "")
    {
        int limit = 10;
        int offset = (pageActual - 1) * limit;
        //Se obtienen la lista de especies, en la cual siempre se hara la consultas
        var pokemonSpeciesList = await _pokemonService.GetPokemonSpecieList();
        if (!string.IsNullOrEmpty(searchPokemon))
        {
            //Se obtiene el pokemon por nombre del pokemon
            var pokemon = await _pokemonService.GetPokemonByName(searchPokemon);
            if (pokemon != null)
            {
                List<PokemonModel> listPokemon = new List<PokemonModel>();
                //Si la especie del pokemon coincide con el filtro o no hay filtro se agrega a la lista
                if (!string.IsNullOrEmpty(pokemonSpecieName) && pokemon.Species.Name.Equals(pokemonSpecieName))
                {
                    listPokemon.Add(pokemon);
                }
                else if (string.IsNullOrEmpty(pokemonSpecieName))
                {
                    listPokemon.Add(pokemon);
                }
                Pagination<PokemonModel> listPokemons = new(listPokemon, listPokemon.Count, pageActual, limit);
                PokemonIndexViewModel viewModel = new PokemonIndexViewModel
                {
                    Pagination = listPokemons,
                    SearchPokemon = searchPokemon,
                    PokemonSpecieName = pokemonSpecieName,
                    PokemonSpeciesListResponse = pokemonSpeciesList,

                };
                return viewModel;
            }
            else
            {
                //Si no se encuentra el pokemon se retorna una lista vacia
                Pagination<PokemonModel> listPokemons = new(new(), 0, pageActual, limit);
                PokemonIndexViewModel viewModel = new PokemonIndexViewModel
                {
                    Pagination = listPokemons,
                    SearchPokemon = searchPokemon,
                    PokemonSpecieName = pokemonSpecieName,
                    PokemonSpeciesListResponse = pokemonSpeciesList,

                };
                return viewModel;
            }
        }
        if (string.IsNullOrEmpty(pokemonSpecieName))
        {
            //Se obtiene la lista completa de pokemons dependiendo el limite y los elementos a saltar offset
            var list = await _pokemonService.GetList(limit, offset);
            //Se obtiene el listado de pokemones por url
            var taskPokemosList = list.Results.Select(p => _pokemonService.GetPokemonByUrl(p.Url));
            var pokemons = await Task.WhenAll(taskPokemosList);

            Pagination<PokemonModel> listPokemons = new(pokemons.ToList(), list.Count, pageActual, limit);
            PokemonIndexViewModel viewModel = new PokemonIndexViewModel
            {
                Pagination = listPokemons,
                SearchPokemon = searchPokemon,
                PokemonSpecieName = pokemonSpecieName,
                PokemonSpeciesListResponse = pokemonSpeciesList,

            };
            return viewModel;
        }
        else
        {
            //Si la espe
            var pokemonSpecie = await _pokemonService.GetPokemonSpecie(pokemonSpecieName);
            var taskPokemonsList = pokemonSpecie.Varieties.Select(v => _pokemonService.GetPokemonByUrl(v.Pokemon.Url));
            var pokemons = await Task.WhenAll(taskPokemonsList);
            Pagination<PokemonModel> listPokemons = new(pokemons.Skip(offset).Take(limit).ToList(), pokemonSpecie.Varieties.Count, pageActual, limit);
            PokemonIndexViewModel viewModel = new PokemonIndexViewModel
            {
                Pagination = listPokemons,
                SearchPokemon = searchPokemon,
                PokemonSpecieName = pokemonSpecieName,
                PokemonSpeciesListResponse = pokemonSpeciesList,

            };
            return viewModel;
        }
    }
    [HttpGet]
    public IActionResult _ShowModalEmail(int pageActual = 1, string searchPokemon = "", string pokemonSpecieName = "")
    {
        return PartialView("_ModalEmail", new PokemonModalEmailViewModel()
        {
            ActualPage = pageActual,
            SearchPokemon=searchPokemon,
            PokemonSpecieName = pokemonSpecieName,
            FileName = $"Lista_Pokemons_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        });

    }
    [HttpPost]
    public async Task<IActionResult> SendEmailMessage(PokemonModalEmailViewModel pokemon)
    {
        try
        {
            var result = await LoadData(pokemon.ActualPage, pokemon.SearchPokemon, pokemon.PokemonSpecieName);
            using var file = GetFileExcel(result.Pagination.Items);
            var emailToSend = new MimeMessage();
            string emailFrom = _configuration["EmailFrom"];
            string host = _configuration["Host"];
            string userAutentication = _configuration["UserAuthentication"];
            string password = _configuration["Password"];
            int port = _configuration.GetValue<int>("Port");
            emailToSend.From.Add(MailboxAddress.Parse(emailFrom));
            emailToSend.To.Add(MailboxAddress.Parse(pokemon.UserRemitentEmail));
            emailToSend.Subject = pokemon.Subject; 
            var builder = new BodyBuilder { TextBody = pokemon.Comment };
            builder.Attachments.Add(pokemon.FileName, file, new ContentType("application","vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
            emailToSend.Body = builder.ToMessageBody();
            using var client = new SmtpClient();
            client.Connect(host, port, MailKit.Security.SecureSocketOptions.SslOnConnect);
            client.Authenticate(userAutentication, password);
            client.Send(emailToSend);
            TempData["Success"] = "Correo enviado";
            return RedirectToAction("Index");
        }
        catch(Exception ex)
        {
            TempData["Error"] = "Error al enviar correo " + ex.Message;
            return RedirectToAction("Index");
        } 
        
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
