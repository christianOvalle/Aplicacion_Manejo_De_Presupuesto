﻿using Dapper;
using Manejo_Presupuesto.Models;
using Microsoft.Data.SqlClient;

namespace Manejo_Presupuesto.Servicios
{
    public interface IRepositorioUsuarios
    {
        Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado);
        Task<int> CrearUsuario(Usuario usuario);
    }

    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly string connectionString;

        public RepositorioUsuarios(IConfiguration configuration) 
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CrearUsuario(Usuario usuario)
        {
            using var connection = new SqlConnection(connectionString);
            var usuarioId = await connection.QuerySingleAsync<int>(@"insert into Usuarios(Email, EmailNormalizado, PasswordHash)
                                                            values (@Email, @EmailNormalizado, @PasswordHash);
                                                            select scope_identity();",usuario);
            await connection.ExecuteAsync("CrearDatosUsuarioNuevo", new { usuarioId }, commandType: System.Data.CommandType.StoredProcedure);

            return usuarioId;
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<Usuario>
            ("select * from Usuarios where EmailNormalizado = @emailNormalizado", new { emailNormalizado });
        }
    }
}
