using Microsoft.AspNetCore.Mvc;
using GestionTareasAPI.DTOs;

namespace GestionTareasAPI.Controllers
{
    [ApiController]
    [Route("api/tareas-externas")]
    public class TareasExternasController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private const string URL_EXTERNA = "https://jsonplaceholder.typicode.com/todos";

        public TareasExternasController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // GET /api/tareas-externas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TareaExternaDTO>>> GetTareasExternas()
        {
            try
            {
                var response = await _httpClient.GetAsync(URL_EXTERNA);

                if (!response.IsSuccessStatusCode)
                    return StatusCode(502, new { mensaje = "La API externa no respondio correctamente" });

                var todos = await response.Content.ReadFromJsonAsync<List<TodoExternoDTO>>();

                if (todos == null)
                    return StatusCode(502, new { mensaje = "No se pudo leer la respuesta de la API externa" });

                var resultado = todos.Select(t => new TareaExternaDTO
                {
                    ExternalId = t.Id,
                    Titulo = t.Title,
                    Completado = t.Completed
                });

                return Ok(resultado);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, new { mensaje = "No se pudo conectar con la API externa. Intente mas tarde." });
            }
        }

        // GET /api/tareas-externas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TareaExternaDTO>> GetTareaExterna(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{URL_EXTERNA}/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound(new { mensaje = $"No se encontro la tarea externa con ID {id}" });

                if (!response.IsSuccessStatusCode)
                    return StatusCode(502, new { mensaje = "La API externa no respondio correctamente" });

                var todo = await response.Content.ReadFromJsonAsync<TodoExternoDTO>();

                if (todo == null)
                    return NotFound(new { mensaje = $"No se encontro la tarea externa con ID {id}" });

                return Ok(new TareaExternaDTO
                {
                    ExternalId = todo.Id,
                    Titulo = todo.Title,
                    Completado = todo.Completed
                });
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, new { mensaje = "No se pudo conectar con la API externa. Intente mas tarde." });
            }
        }
    }

    // DTO interno para deserializar la respuesta de jsonplaceholder
    internal class TodoExternoDTO
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool Completed { get; set; }
    }
}
