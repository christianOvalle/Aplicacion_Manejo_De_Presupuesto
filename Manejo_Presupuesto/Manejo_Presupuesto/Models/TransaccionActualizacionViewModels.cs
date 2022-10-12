namespace Manejo_Presupuesto.Models
{
    public class TransaccionActualizacionViewModels : TransaccionCreacionViewModels
    {
        public int CuentaAnteriorId { get; set; }   

        public decimal MontoAnterior { get; set; }

        public string UrlRetorno { get; set; }
    }
}
