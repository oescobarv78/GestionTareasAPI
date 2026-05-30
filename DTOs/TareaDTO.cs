using System.ComponentModel.DataAnnotations;
using GestionTareasAPI.Models;

namespace GestionTareasAPI.DTOs
{
    public class CrearTareaDTO
    {
        [Required(ErrorMessage = "El titulo es obligatorio")]
        [StringLength(200, MinimumLength = 1)]
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        [Required(ErrorMessage = "El estado es obligatorio")]
        public EstadoTarea Estado { get; set; }
        [Required(ErrorMessage = "La prioridad es obligatoria")]
        public PrioridadTarea Prioridad { get; set; }
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        public DateTime FechaVencimiento { get; set; }
    }

    public class ActualizarTareaDTO
    {
        [Required(ErrorMessage = "El titulo es obligatorio")]
        [StringLength(200, MinimumLength = 1)]
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        [Required(ErrorMessage = "El estado es obligatorio")]
        public EstadoTarea Estado { get; set; }
        [Required(ErrorMessage = "La prioridad es obligatoria")]
        public PrioridadTarea Prioridad { get; set; }
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        public DateTime FechaVencimiento { get; set; }
    }

    public class TareaResponseDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Prioridad { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }
}
