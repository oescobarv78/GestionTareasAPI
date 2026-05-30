namespace GestionTareasAPI.Models
{
    public class Tarea
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public EstadoTarea Estado { get; set; }
        public PrioridadTarea Prioridad { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaVencimiento { get; set; }
    }

    public enum EstadoTarea
    {
        Pendiente,
        EnProceso,
        Completada
    }

    public enum PrioridadTarea
    {
        Baja,
        Media,
        Alta
    }
}
