using Manejo_Presupuesto.Models;
using Manejo_Presupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Manejo_Presupuesto.Controllers
{
    public class CategoriasController : Controller

    {
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IserviciosUsuarios serviciosUsuarios;

        public CategoriasController(IRepositorioCategorias repositorioCategorias, IserviciosUsuarios iserviciosUsuarios)
        {
            this.repositorioCategorias = repositorioCategorias;
            this.serviciosUsuarios = iserviciosUsuarios;
        }

        public async Task<IActionResult> Index() 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var categorias = await repositorioCategorias.Obtener(usuarioId);

            return View(categorias);
        }

        public IActionResult Crear() 
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria) 
        {
            if (!ModelState.IsValid) 
            {
                return View(categoria);
            }

            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            categoria.UsuarioId = usuarioId;
            await repositorioCategorias.Crear(categoria);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id) 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var categoria = await repositorioCategorias.ObtenerPorId(id, usuarioId);

            if(categoria is null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Editar (Categoria categoriaEditar) 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var categoria = repositorioCategorias.ObtenerPorId(categoriaEditar.Id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(categoriaEditar);
            }

            categoriaEditar.UsuarioId = usuarioId;
            await repositorioCategorias.Actualizar(categoriaEditar);
            return RedirectToAction("Index");


        }

        public async Task<IActionResult> Borrar(int id) 
        {

            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var categoria = await repositorioCategorias.ObtenerPorId(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(categoria);
        }
        [HttpPost]
        public async Task<IActionResult> BorrarCategoria(int id) 
        {

            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var categoria = await repositorioCategorias.ObtenerPorId(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrardo", "Home");
            }

            await repositorioCategorias.Borrar(id);
            return RedirectToAction("Index");
        }
    }
}
