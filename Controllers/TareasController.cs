using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionTareasAPI.Data;
using GestionTareasAPI.DTOs;
using GestionTareasAPI.Models;

namespace GestionTareasAPI.Controllers
{
    [ApiController]
    [Route("api/tareas")]
    public class TareasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TareasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TareaResponseDTO>>> GetTareas()
        {
            var tareas = await _context.Tareas.ToListAsync();
            return Ok(tareas.Select(t => MapearATareaResponse(t)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TareaResponseDTO>> GetTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
                return NotFound(new { mensaje = $"No se encontro la tarea con ID {id}" });
            return Ok(MapearATareaResponse(tarea));
        }

        [HttpPost]
        public async Task<ActionResult<TareaResponseDTO>> CrearTarea([FromBody] CrearTareaDTO dto)
        {
            if (dto.FechaVencimiento.Date < DateTime.UtcNow.Date)
                return BadRequest(new { mensaje = "La fecha de vencimiento no puede ser menor a la fecha actual" });

            var tarea = new Tarea
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                Estado = dto.Estado,
                Prioridad = dto.Prioridad,
                FechaCreacion = DateTime.UtcNow,
                FechaVencimiento = dto.FechaVencimiento
            };

            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id }, MapearATareaResponse(tarea));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TareaResponseDTO>> ActualizarTarea(int id, [FromBody] ActualizarTareaDTO dto)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
                return NotFound(new { mensaje = $"No se encontro la tarea con ID {id}" });

            if (dto.FechaVencimiento.Date < DateTime.UtcNow.Date)
                return BadRequest(new { mensaje = "La fecha de vencimiento no puede ser menor a la fecha actual" });

            tarea.Titulo = dto.Titulo;
            tarea.Descripcion = dto.Descripcion;
            tarea.Estado = dto.Estado;
            tarea.Prioridad = dto.Prioridad;
            tarea.FechaVencimiento = dto.FechaVencimiento;

            await _context.SaveChangesAsync();
            return Ok(MapearATareaResponse(tarea));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
                return NotFound(new { mensaje = $"No se encontro la tarea con ID {id}" });

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static TareaResponseDTO MapearATareaResponse(Tarea tarea)
        {
            return new TareaResponseDTO
            {
                Id = tarea.Id,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                Estado = tarea.Estado.ToString(),
                Prioridad = tarea.Prioridad.ToString(),
                FechaCreacion = tarea.FechaCreacion,
                FechaVencimiento = tarea.FechaVencimiento
            };
        }
    }
}
