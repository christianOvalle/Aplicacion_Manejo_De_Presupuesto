using Dapper;
using Manejo_Presupuesto.Models;
using Microsoft.Data.SqlClient;

namespace Manejo_Presupuesto.Servicios
{

    public interface IRepositorioTiposCuentas
    {
        void Crear(TipoCuenta tipoCuenta);
    }

    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        private readonly string connectionString;

        public RepositorioTiposCuentas(IConfiguration configuration) 
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Crear(TipoCuenta tipoCuenta) 
        {
            using var connection = new SqlConnection(connectionString);
            var id = connection.QuerySingle<int>($@"insert into TiposCuentas (Nombre, UsuarioId, Orden)
                                                 Values(@Nombre, @UsuarioId, 0);
                                                 Select Scope_Identity();", tipoCuenta);
        }

    }
}
