using Dapper;
using Manejo_Presupuesto.Models;
using Microsoft.Data.SqlClient;

namespace Manejo_Presupuesto.Servicios
{

    public interface IRepositorioCategorias
    {
        Task Actualizar(Categoria categoria);
        Task Borrar(int id);
        Task Crear(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId);
        Task<Categoria> ObtenerPorId(int Id, int UsuarioId);
    }


    public class RepositorioCategorias : IRepositorioCategorias
    {
        public readonly string connectionString;

        public RepositorioCategorias(IConfiguration configuration) 
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Categoria categoria) 
        {
            using var connention = new SqlConnection(connectionString);
            var id = await connention.QuerySingleAsync<int>(@"insert into Categorias(Nombre, TipoOperacionId, UsuarioId)
                                                            values (@Nombre, @TipoOperacionId, @UsuarioId)
                                                            select SCOPE_IDENTITY();", categoria);

            categoria.Id = id;
        }
        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>(@"select *from Categorias where UsuarioId = @UsuarioId", new { usuarioId });

        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>(@"select *from Categorias where UsuarioId = @UsuarioId and TipoOperacionId = @tipoOperacionId", new { usuarioId, tipoOperacionId });

        }

        public async Task<Categoria> ObtenerPorId(int Id, int UsuarioId) 
        {
            using var connention = new SqlConnection(connectionString);
            return await connention.QueryFirstOrDefaultAsync<Categoria>(@"select *from Categorias where Id = @Id and UsuarioId = @UsuarioId", new { Id, UsuarioId });
        }

        public async Task Actualizar (Categoria categoria) 
        {
            using var connention = new SqlConnection(connectionString);
            await connention.ExecuteAsync(@"update Categorias set Nombre = @Nombre, TipoOperacionId = @TiOperacionId
                                          where Id = @Id", categoria);
        }

        public async Task Borrar(int id)
        {
            using var connention = new SqlConnection(connectionString);
            await connention.ExecuteAsync(@"delete Categorias where Id= @Id", new { id });

        }
    }
}
 