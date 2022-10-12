namespace Manejo_Presupuesto.Models
{
    public class ObtenerTransaccionesPorCuenta
    {
        public int UsuarioId { get; set; }

        public int CuentasId { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }
    }
}
