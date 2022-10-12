using Dapper;
using Manejo_Presupuesto.Models;
using Microsoft.Data.SqlClient;

namespace Manejo_Presupuesto.Servicios
{
    public interface IRepositorioCuentas
    {
        Task Actualizar(CuentaCreacionViewModels cuenta);
        Task Borrar(int id);
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task Crear(Cuenta cuenta);
        Task<Cuenta> ObtenerPorId(int id, int usuarioId);
    }

    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string connectionString;

        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");

        }
        public async Task Crear(Cuenta cuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"insert into Cuentas (Nombre, TipoCuentaId, Descripcion, Balance )
                                                 values (@Nombre, @TipoCuentaId, @Descripcion, @Balance )
                                                 select SCOPE_IDENTITY();", cuenta);

            cuenta.Id = id;

        }

        
        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuenta>(@"select Cuentas.Id, Cuentas.Nombre, Balance, tc.Nombre as TipoCuenta
                                                        from Cuentas
                                                        inner join TiposCuentas tc
                                                        on tc.Id = Cuentas.TipoCuentaId
                                                        where tc.UsuarioId = @usuarioId
                                                        order by tc.Orden", new { usuarioId });
        }

        public async Task<Cuenta> ObtenerPorId(int id, int usuarioId) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cuenta>(
                @"select Cuentas.Id, Cuentas.Nombre, Balance, Descripcion, TipoCuentaId
                  from Cuentas
                  inner join TiposCuentas tc
                  on tc.Id = Cuentas.TipoCuentaId
                  where tc.UsuarioId = @usuarioId and Cuentas.Id = @Id", new {id,usuarioId});
        }

        public async Task Actualizar(CuentaCreacionViewModels cuenta) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"update Cuentas
                                           set Nombre = @Nombre, Balance = @Balance, Descripcion = @Descripcion,
                                           TipoCuentaId = TipoCuentaId
                                           where id = @Id;", cuenta);
        }

        public async Task Borrar(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Cuentas where Id = @Id", new { id });
        }
    }
}
