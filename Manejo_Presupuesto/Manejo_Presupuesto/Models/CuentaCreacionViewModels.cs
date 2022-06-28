using Microsoft.AspNetCore.Mvc.Rendering;

namespace Manejo_Presupuesto.Models
{
    public class CuentaCreacionViewModels : Cuenta
    {
        public IEnumerable<SelectListItem> TiposCuentas { get; set; }
    }
}
