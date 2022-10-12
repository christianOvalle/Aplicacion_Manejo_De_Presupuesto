using Dapper;
using Manejo_Presupuesto.Models;
using Microsoft.Data.SqlClient;

namespace Manejo_Presupuesto.Servicios
{
    public interface IRepositorioTransacciones
    {
        Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnterior);
        Task Borrar(int id);
        Task Crear(Transaccion transaccion);
        Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo);
        Task<Transaccion> ObtenerPorId(int id, int usuarioId);
        Task<IEnumerable<Transaccion>> ObtenerPorUsuarioId(ParametroObtenerTransaccionesPorUsuario modelo);
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
            var id = await connection.QuerySingleAsync<int>("Transaccione_Insertar",
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

        public async Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo)
        {
            using var connection = new SqlConnection(connectiponString);
            return await connection.QueryAsync<Transaccion>(@"select t.Id, t.Monto, t.FechaTransaccion, c.Nombre as Categoria,
                                                            cu.Nombre as Cuenta, c.TipoOperacionId
                                                            from Transacciones t
                                                            inner join Categorias c
                                                            on c.Id = t.CategoriaId
                                                            inner join Cuentas cu
                                                            on cu.Id = t.CuentasId
                                                            where t.CuentasId = @CuentasId and t.UsuarioId = @UsuarioId
                                                            and FechaTransaccion between @FechaInicio and @FechaFin", modelo);
        }

        public async Task<IEnumerable<Transaccion>> ObtenerPorUsuarioId(ParametroObtenerTransaccionesPorUsuario modelo)
        {
            using var connection = new SqlConnection(connectiponString);
            return await connection.QueryAsync<Transaccion>(@"select t.Id, t.Monto, t.FechaTransaccion, c.Nombre as Categoria,
                                                            cu.Nombre as Cuenta, c.TipoOperacionId
                                                            from Transacciones t
                                                            inner join Categorias c
                                                            on c.Id = t.CategoriaId
                                                            inner join Cuentas cu
                                                            on cu.Id = t.CuentasId
                                                            where t.UsuarioId = @UsuarioId
                                                            and FechaTransaccion between @FechaInicio and @FechaFin
                                                            order by t.FechaTransaccion desc", modelo);
        }

        public async Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnteriorId)
        {
            using var connection = new SqlConnection(connectiponString);
            await connection.ExecuteAsync("Transacciones_Actualizar",
                new
                {
                    transaccion.Id,
                    transaccion.FechaTransaccion,
                    transaccion.Monto,
                    transaccion.CategoriaId,
                    transaccion.CuentaId,
                    transaccion.Nota,
                    montoAnterior,
                    cuentaAnteriorId

                }, commandType: System.Data.CommandType.StoredProcedure);
        }
        public async Task<Transaccion> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectiponString);
            return await connection.QueryFirstOrDefaultAsync<Transaccion>(@"
             Select Transacciones.*, cat.TipoOperacionId from Transacciones
             Inner Join Categorias cat
             on cat.Id = Transacciones.CategoriaId
             where Transacciones.Id = @Id and Transacciones.UsuarioId = @UsuarioId",
             new
             {
                 id,
                 usuarioId
             });
        }

        public async Task Borrar (int id)
        {
            using var connection = new SqlConnection(connectiponString);
            await connection.ExecuteAsync("Transacciones_Borrar",
                new {id}, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}