using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using PowerGuard.Estrutura.Model;

namespace PowerGuard.Estrutura.Repository
{
    public class SimulacaoRepository : BaseRepository
    {
        public void Inserir(Simulacao sim)
        {
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = @"INSERT INTO SIMULACAO 
                  (TIPO_SIMULACAO, DESCRICAO, DATA_HORA, CONCLUIDA) 
                  VALUES (:tipo, :descricao, :datahora, :concluida)";

            using var cmd = new OracleCommand(sql, con);
            cmd.Parameters.Add("tipo", sim.TipoSimulacao);
            cmd.Parameters.Add("descricao", sim.Descricao);
            cmd.Parameters.Add("datahora", sim.DataHora);
            cmd.Parameters.Add("concluida", sim.Concluida ? 1 : 0);

            cmd.ExecuteNonQuery();
        }


        public List<Simulacao> ListarTodos()
        {
            var lista = new List<Simulacao>();
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = "SELECT * FROM SIMULACAO ORDER BY ID";
            using var cmd = new OracleCommand(sql, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Simulacao
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    TipoSimulacao = reader["TIPO_SIMULACAO"].ToString(),
                    Descricao = reader["DESCRICAO"].ToString(),
                    DataHora = Convert.ToDateTime(reader["DATA_HORA"]),
                    Concluida = Convert.ToInt32(reader["CONCLUIDA"]) == 1
                });
            }

            return lista;
        }

        public Simulacao ObterPorId(int id)
        {
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = "SELECT * FROM SIMULACAO WHERE ID = :id";
            using var cmd = new OracleCommand(sql, con);
            cmd.Parameters.Add("id", id);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Simulacao
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    TipoSimulacao = reader["TIPO_SIMULACAO"].ToString(),
                    Descricao = reader["DESCRICAO"].ToString(),
                    DataHora = Convert.ToDateTime(reader["DATA_HORA"]),
                    Concluida = Convert.ToInt32(reader["CONCLUIDA"]) == 1
                };
            }

            throw new Exception("Simulação não encontrada.");
        }

        public void Atualizar(Simulacao sim)
        {
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = @"UPDATE SIMULACAO SET 
                          TIPO_SIMULACAO = :tipo, DESCRICAO = :desc, 
                          DATA_HORA = :data, CONCLUIDA = :concluida 
                          WHERE ID = :id";
            using var cmd = new OracleCommand(sql, con);
            cmd.Parameters.Add("tipo", sim.TipoSimulacao);
            cmd.Parameters.Add("desc", sim.Descricao);
            cmd.Parameters.Add("data", sim.DataHora);
            cmd.Parameters.Add("concluida", sim.Concluida ? 1 : 0);
            cmd.Parameters.Add("id", sim.Id);

            cmd.ExecuteNonQuery();
        }
    }
}

