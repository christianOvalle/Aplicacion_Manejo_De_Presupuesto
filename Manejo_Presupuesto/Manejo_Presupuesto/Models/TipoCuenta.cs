using Manejo_Presupuesto.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace Manejo_Presupuesto.Models
{
    public class TipoCuenta
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo nombre es requerido")]
        [ValidandoPrimeraLetra]

        public string Nombre { get; set; }

        public int UsuarioId { get; set; }

        public int Orden { get; set; }

    }
}
