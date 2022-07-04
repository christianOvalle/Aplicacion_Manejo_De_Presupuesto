using Dapper;
using Manejo_Presupuesto.Models;
using Microsoft.Data.SqlClient;

namespace Manejo_Presupuesto.Servicios
{
    public interface IRepositorioTransacciones
    {
        Task Crear(Transaccion transaccion);
    }

    public class RepositorioTransacciones : IRepositorioTransacciones
    {
        private readonly string connectiponString;

        public RepositorioTransacciones(IConfiguration configuration)
        {
            connectiponString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Transaccion transaccion)
        {
            using var connection = new SqlConnection(connectiponString);
            var id = await connection.QuerySingleAsync<int>("Transacciones_Insertar",
                new
                {
                    transaccion.UsuarioId,
                    transaccion.FechaTransaccion,
                    transaccion.Monto,
                    transaccion.CategoriaId,
                    transaccion.CuentaId,
                    transaccion.Nota
                }, commandType: System.Data.CommandType.StoredProcedure);

            transaccion.Id = id;
               
        }
    }
}
