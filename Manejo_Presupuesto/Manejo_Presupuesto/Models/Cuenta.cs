using Manejo_Presupuesto.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace Manejo_Presupuesto.Models
{
    public class Cuenta
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        [StringLength(maximumLength:50)]
        [ValidandoPrimeraLetra(ErrorMessage ="La primera letra debe ser mayuscula")]

        public string Nombre { get; set; }
        [Display(Name ="Tipo Cuenta")]

        public int TipoCuentaId { get ; set; }

        public decimal Balance { get; set; }
        [StringLength(maximumLength:1000)]
        public string Descripcion { get; set; }

        public string TipoCuenta { get; set; }
    }
}
