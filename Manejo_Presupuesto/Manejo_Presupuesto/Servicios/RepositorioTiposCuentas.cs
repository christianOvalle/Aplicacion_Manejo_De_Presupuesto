using Dapper;
using Manejo_Presupuesto.Models;
using Microsoft.Data.SqlClient;

namespace Manejo_Presupuesto.Servicios
{

    public interface IRepositorioTiposCuentas
    {
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Borrar(int id);
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task<TipoCuenta> ObtenerPorId(int id, int usuariodi);
        Task Ordenar(IEnumerable<TipoCuenta> TipoCuentasOrdenar);
    }



    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        private readonly string connectionString;


        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>("TiposCuentasOrden", new {usuarioId = tipoCuenta.UsuarioId,
                                                                                 nombre = tipoCuenta.Nombre},
                                                                                 commandType: System.Data.CommandType.StoredProcedure);

            tipoCuenta.Id = id;
        }

        public async Task<bool> Existe(string nombre, int usuarioId) {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(
                                       @"select 1
                                    from TiposCuentas
                                    where Nombre = @Nombre and UsuarioId = @UsuarioId;",
                                        new { nombre, usuarioId });
            return existe == 1;

        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"select Id, Nombre, Orden
                                                           from TiposCuentas
                                                           where UsuarioId = @UsuarioId
                                                           Order BY Orden", new { usuarioId });
        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"update TiposCuentas
                                            set Nombre = @Nombre
                                            where Id = @Id", tipoCuenta);
        }

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioid)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"select Id,Nombre, Orden
                                                                          from TiposCuentas
                                                                          where Id=@Id and UsuarioId = @UsuarioId",
                                                                          new { id, usuarioid });
        }

        public async Task Borrar(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"Delete TiposCuentas where id = @id", new {id});
        }

        public async Task Ordenar (IEnumerable<TipoCuenta> TipoCuentasOrdenar) 
        {
            var query = "update TiposCuentas SET Orden = @Orden where Id = @Id";
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, TipoCuentasOrdenar);
        }


    }
}
