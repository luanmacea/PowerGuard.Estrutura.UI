using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using PowerGuard.Estrutura.Model;

namespace PowerGuard.Estrutura.Repository
{
    public class LogEventoRepository : BaseRepository
    {
        public void Inserir(LogEvento log)
        {
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = @"INSERT INTO LOG_EVENTO (ACAO, DATA_HORA, USUARIO_ID) 
                           VALUES (:acao, :datahora, :usuarioId)";
            using var cmd = new OracleCommand(sql, con);
            cmd.Parameters.Add("acao", log.Acao);
            cmd.Parameters.Add("datahora", log.DataHora);
            cmd.Parameters.Add("usuarioId", log.UsuarioId);

            cmd.ExecuteNonQuery();
        }

        public List<LogEvento> ListarTodos()
        {
            var lista = new List<LogEvento>();
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = "SELECT * FROM LOG_EVENTO ORDER BY ID";
            using var cmd = new OracleCommand(sql, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new LogEvento
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    Acao = reader["ACAO"].ToString(),
                    DataHora = Convert.ToDateTime(reader["DATA_HORA"]),
                    UsuarioId = Convert.ToInt32(reader["USUARIO_ID"])
                });
            }

            return lista;
        }
    }
}

