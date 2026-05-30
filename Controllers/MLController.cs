using Microsoft.AspNetCore.Mvc;
using GestionTareasAPI.MLModels;

namespace GestionTareasAPI.Controllers
{
    [ApiController]
    [Route("api/ml")]
    public class MLController : ControllerBase
    {
        private readonly SentimientoService _sentimientoService;

        public MLController(SentimientoService sentimientoService)
        {
            _sentimientoService = sentimientoService;
        }

        [HttpPost("sentimiento")]
        public ActionResult AnalizarSentimiento([FromBody] SentimientoRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Comentario))
                return BadRequest(new { mensaje = "El comentario no puede estar vacio" });

            var sentimiento = _sentimientoService.AnalizarSentimiento(request.Comentario);

            return Ok(new
            {
                comentario = request.Comentario,
                sentimiento = sentimiento
            });
        }
    }

    public class SentimientoRequestDTO
    {
        public string Comentario { get; set; } = string.Empty;
    }
}
