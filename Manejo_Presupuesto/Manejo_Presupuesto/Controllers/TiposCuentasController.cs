using Manejo_Presupuesto.Models;
using Manejo_Presupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Manejo_Presupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IserviciosUsuarios serviciosUsuarios;

        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IserviciosUsuarios iserviciosUsuarios)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.serviciosUsuarios = iserviciosUsuarios;
        }

        public async Task<IActionResult> Index() 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var tiposcuenta = await repositorioTiposCuentas.Obtener(usuarioId);
            return View(tiposcuenta);

        }


        public IActionResult Crear()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta) 
        {
            if (!ModelState.IsValid) {

                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = serviciosUsuarios.ObtenerUsuarioId();

            var ExisteCuenta = await repositorioTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (ExisteCuenta) 
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre{tipoCuenta.Nombre} ya existe");

                return View(tipoCuenta);
            }


           await repositorioTiposCuentas.Crear(tipoCuenta);
        
        
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id) 
        {
            var usuarioid = serviciosUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(id, usuarioid);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoCuenta);
        }
        [HttpPost]

        public async Task<IActionResult>Editar(TipoCuenta tipoCuenta)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var CuentaExiste = await repositorioTiposCuentas.ObtenerPorId(tipoCuenta.Id, usuarioId);

            if (CuentaExiste is null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioTiposCuentas.Actualizar(tipoCuenta);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Borrar(int id) 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var tipocuenta = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            if (tipocuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipocuenta);
        }
        [HttpPost]
        public async Task<IActionResult> BorraTipoCuenta(int id) 
        {

            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var tipocuenta = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            if (tipocuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioTiposCuentas.Borrar(id);
            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> Ordenar([FromBody] int[] ids) 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
            var idsTiposCuentas = tiposCuentas.Select(x => x.Id);

            var idsTiposCuentasNoPerteneceAlUsuario = ids.Except(idsTiposCuentas).ToList();

            if (idsTiposCuentasNoPerteneceAlUsuario.Count > 0) 
            {
                return Forbid();
            }

            var tiposCuentasOrdenados = ids.Select((valor,indice) =>
            new TipoCuenta() { Id = valor, Orden = indice + 1 }).AsEnumerable();

            await repositorioTiposCuentas.Ordenar(tiposCuentasOrdenados);

            return Ok();
        }

    }
}
