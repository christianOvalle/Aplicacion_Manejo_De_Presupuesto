using Manejo_Presupuesto.Models;
using Manejo_Presupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Manejo_Presupuesto.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IserviciosUsuarios serviciosUsuarios;
        private readonly IRepositorioCuentas repositorioCuentas;

        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IserviciosUsuarios iserviciosUsuarios, IRepositorioCuentas repositorioCuentas)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.serviciosUsuarios = iserviciosUsuarios;
            this.repositorioCuentas = repositorioCuentas;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var cuentasConTipoCuenta = await repositorioCuentas.Buscar(usuarioId);

            var modelo = cuentasConTipoCuenta
                .GroupBy(x => x.TipoCuenta)
                .Select(grupo => new IndiceCuentaViewModel
                {
                    TipoCuenta = grupo.Key,
                    Cuentas = grupo.AsEnumerable()
                }).ToList();

            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var modelo = new CuentaCreacionViewModels();
            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionViewModels cuenta)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(cuenta.TipoCuentaId, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid)

            {
                cuenta.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
                return View(cuenta);

            }
            await repositorioCuentas.Crear(cuenta);
            return RedirectToAction("Index");

        }
        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId){

         var tipoCuentas = await repositorioTiposCuentas.Obtener(usuarioId);

        return tipoCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
    } }
}