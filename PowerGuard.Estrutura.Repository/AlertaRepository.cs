using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using PowerGuard.Estrutura.Model;

namespace PowerGuard.Estrutura.Repository
{
    public class AlertaRepository : BaseRepository
    {
        public void Inserir(Alerta alerta)
        {
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = @"INSERT INTO ALERTA 
                          (MENSAGEM, DATA_HORA, USUARIO_ID, FALHA_ENERGIA_ID) 
                          VALUES (:msg, :data, :usuarioId, :falhaId)";
            using var cmd = new OracleCommand(sql, con);
            cmd.Parameters.Add("msg", alerta.Mensagem);
            cmd.Parameters.Add("data", alerta.DataHora);
            cmd.Parameters.Add("usuarioId", alerta.UsuarioId);
            cmd.Parameters.Add("falhaId", alerta.FalhaEnergiaId ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public List<Alerta> ListarTodos()
        {
            var lista = new List<Alerta>();
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = "SELECT * FROM ALERTA ORDER BY ID";
            using var cmd = new OracleCommand(sql, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Alerta
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    Mensagem = reader["MENSAGEM"].ToString(),
                    DataHora = Convert.ToDateTime(reader["DATA_HORA"]),
                    UsuarioId = Convert.ToInt32(reader["USUARIO_ID"]),
                    FalhaEnergiaId = reader["FALHA_ENERGIA_ID"] == DBNull.Value ? null : Convert.ToInt32(reader["FALHA_ENERGIA_ID"])
                });
            }

            return lista;
        }

        public void Remover(int id)
        {
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = "DELETE FROM ALERTA WHERE ID = :id";
            using var cmd = new OracleCommand(sql, con);
            cmd.Parameters.Add("id", id);
            cmd.ExecuteNonQuery();
        }
    }
}

