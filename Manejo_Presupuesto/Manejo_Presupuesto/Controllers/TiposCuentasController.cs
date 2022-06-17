using Manejo_Presupuesto.Models;
using Manejo_Presupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Manejo_Presupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas)
        {
            RepositorioTiposCuentas = repositorioTiposCuentas;
        }

        public IRepositorioTiposCuentas RepositorioTiposCuentas { get; }

        public IActionResult Crear()
        {
           

            return View();
        }
        [HttpPost]
        public IActionResult Crear(TipoCuenta tipoCuenta) 
        {
            if (!ModelState.IsValid) {

                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = 1;
            RepositorioTiposCuentas.Crear(tipoCuenta);
        
        
            return View();
        }


    }
}
