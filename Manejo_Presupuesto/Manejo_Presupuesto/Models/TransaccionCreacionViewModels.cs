using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Manejo_Presupuesto.Models
{
    public class TransaccionCreacionViewModels : Transaccion
    {
        public IEnumerable<SelectListItem> Cuentas  { get; set; }

        public IEnumerable<SelectListItem> Categorias { get; set; }
        [Display(Name ="Tipo Operacion")]
        public TipoOperacion TipoOperacionId { get; set; }  
    }
}
