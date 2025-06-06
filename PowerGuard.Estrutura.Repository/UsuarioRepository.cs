using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using PowerGuard.Estrutura.Model;
using System.Data;

namespace PowerGuard.Estrutura.Repository
{
    public class UsuarioRepository : BaseRepository
    {
        public Usuario? ValidarLogin(string email, string senha)
        {
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = "SELECT * FROM USUARIO WHERE EMAIL = :email AND SENHA = :senha";
            using var cmd = new OracleCommand(sql, con);
            cmd.Parameters.Add("email", email);
            cmd.Parameters.Add("senha", senha);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    Nome = reader["NOME"].ToString() ?? "",
                    Email = reader["EMAIL"].ToString() ?? "",
                    Senha = "",
                    IsAdmin = Convert.ToInt32(reader["IS_ADMIN"]) == 1
                };
            }

            return null;
        }

        public void Inserir(Usuario usuario)
        {
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = "INSERT INTO USUARIO (NOME, EMAIL, SENHA, IS_ADMIN) VALUES (:nome, :email, :senha, :admin)";
            using var cmd = new OracleCommand(sql, con);
            cmd.Parameters.Add("nome", usuario.Nome);
            cmd.Parameters.Add("email", usuario.Email);
            cmd.Parameters.Add("senha", usuario.Senha);
            cmd.Parameters.Add("admin", usuario.IsAdmin ? 1 : 0);

            cmd.ExecuteNonQuery();
        }

        public List<Usuario> ListarTodos()
        {
            var lista = new List<Usuario>();
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = "SELECT ID, NOME, EMAIL, SENHA, IS_ADMIN FROM USUARIO ORDER BY ID";
            using var cmd = new OracleCommand(sql, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Usuario
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    Nome = reader["NOME"].ToString(),
                    Email = reader["EMAIL"].ToString(),
                    Senha = reader["SENHA"].ToString(),
                    IsAdmin = Convert.ToInt32(reader["IS_ADMIN"]) == 1
                });
            }

            return lista;
        }

        public void Atualizar(Usuario usuario)
        {
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = "UPDATE USUARIO SET NOME = :nome, EMAIL = :email, SENHA = :senha, IS_ADMIN = :admin WHERE ID = :id";
            using var cmd = new OracleCommand(sql, con);
            cmd.Parameters.Add("nome", usuario.Nome);
            cmd.Parameters.Add("email", usuario.Email);
            cmd.Parameters.Add("senha", usuario.Senha);
            cmd.Parameters.Add("admin", usuario.IsAdmin ? 1 : 0);
            cmd.Parameters.Add("id", usuario.Id);

            cmd.ExecuteNonQuery();
        }

        public void Remover(int id)
        {
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = "DELETE FROM USUARIO WHERE ID = :id";
            using var cmd = new OracleCommand(sql, con);
            cmd.Parameters.Add("id", id);
            cmd.ExecuteNonQuery();
        }
    }
}

