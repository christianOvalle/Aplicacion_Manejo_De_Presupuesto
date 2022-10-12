using System.Security.Claims;

namespace Manejo_Presupuesto.Servicios
{
    public interface IserviciosUsuarios
    {
        int ObtenerUsuarioId();
    }

    public class ServicioUsuarios : IserviciosUsuarios
    {
        private readonly HttpContext httpContext;

        public ServicioUsuarios(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }

        public int ObtenerUsuarioId()
        {
            if (httpContext.User.Identity.IsAuthenticated) 
            {
                var idClaim = httpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var id = int.Parse(idClaim.Value);
                return 1;
            }
            else
            {
                throw new ApplicationException("El usuario no esta autenticado");
            }
        }
       
    }
}
