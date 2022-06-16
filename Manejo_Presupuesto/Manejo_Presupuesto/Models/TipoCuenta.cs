using System.ComponentModel.DataAnnotations;

namespace Manejo_Presupuesto.Models
{
    public class TipoCuenta
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo nombre es requerido")]
        [StringLength(maximumLength: 40, MinimumLength = 3, ErrorMessage ="La longitud del campo nombre debe estar entre 50 y 3") ]
        public string Nombre { get; set; }

        public int UsuarioId { get; set; }

        public int Orden { get; set; }

    }
}
