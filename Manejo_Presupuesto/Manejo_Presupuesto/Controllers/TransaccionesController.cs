using Manejo_Presupuesto.Models;
using Manejo_Presupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Manejo_Presupuesto.Controllers
{
    public class TransaccionesController : Controller
    {
        private readonly IRepositorioCuentas repositorioCuentas;
        private readonly IserviciosUsuarios serviciosUsuarios;
        private readonly IRepositorioCategorias repositorioCategorias;

        public TransaccionesController(IRepositorioCuentas repositorioCuentas, IserviciosUsuarios iserviciosUsuarios, IRepositorioCategorias repositorioCategorias)
        {
            this.repositorioCuentas = repositorioCuentas;
            this.serviciosUsuarios = iserviciosUsuarios;
            this.repositorioCategorias = repositorioCategorias;
        }

        public async Task<IActionResult> Crear() 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var modelo = new TransaccionCreacionViewModels();
            modelo.Cuentas = await ObtenerCuentas(usuarioId);
            return View(modelo);

        }
        private async Task<IEnumerable<SelectListItem>> ObtenerCategorias(int UsuarioId, TipoOperacion tipoOperacion)
        {
            var cuentas = await repositorioCategorias.Obtener(UsuarioId, tipoOperacion);
            return cuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerCuentas(int UsuarioId)
        {
            var cuentas = await repositorioCuentas.Buscar(UsuarioId);
            return cuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        public async Task<IActionResult> ObtenerCategorias([FromBody] TipoOperacion tipoOperacion) 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var categorias = await ObtenerCategorias(usuarioId, tipoOperacion);
            return Ok(categorias);
        }
    }
}