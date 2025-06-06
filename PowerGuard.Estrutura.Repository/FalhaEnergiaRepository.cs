using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using PowerGuard.Estrutura.Model;

namespace PowerGuard.Estrutura.Repository
{
    public class FalhaEnergiaRepository : BaseRepository
    {
        public void Inserir(FalhaEnergia falha)
        {
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = @"INSERT INTO FALHA_ENERGIA 
                          (DATA_HORA, LOCALIZACAO, CAUSA_PROVAVEL, TIPO_EVENTO, GRAVIDADE) 
                          VALUES (:datahora, :loc, :causa, :tipo, :grav)";
            using var cmd = new OracleCommand(sql, con);
            cmd.Parameters.Add("datahora", falha.DataHora);
            cmd.Parameters.Add("loc", falha.Localizacao);
            cmd.Parameters.Add("causa", falha.CausaProvavel);
            cmd.Parameters.Add("tipo", falha.TipoEvento);
            cmd.Parameters.Add("grav", falha.Gravidade);

            cmd.ExecuteNonQuery();
        }

        public List<FalhaEnergia> ListarTodos()
        {
            var lista = new List<FalhaEnergia>();
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = "SELECT * FROM FALHA_ENERGIA ORDER BY ID";
            using var cmd = new OracleCommand(sql, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new FalhaEnergia
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    DataHora = Convert.ToDateTime(reader["DATA_HORA"]),
                    Localizacao = reader["LOCALIZACAO"].ToString(),
                    CausaProvavel = reader["CAUSA_PROVAVEL"].ToString(),
                    TipoEvento = reader["TIPO_EVENTO"].ToString(),
                    Gravidade = reader["GRAVIDADE"].ToString()
                });
            }

            return lista;
        }

        public void Atualizar(FalhaEnergia falha)
        {
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = @"UPDATE FALHA_ENERGIA SET 
                          DATA_HORA = :data, LOCALIZACAO = :loc, 
                          CAUSA_PROVAVEL = :causa, TIPO_EVENTO = :tipo, 
                          GRAVIDADE = :grav WHERE ID = :id";
            using var cmd = new OracleCommand(sql, con);
            cmd.Parameters.Add("data", falha.DataHora);
            cmd.Parameters.Add("loc", falha.Localizacao);
            cmd.Parameters.Add("causa", falha.CausaProvavel);
            cmd.Parameters.Add("tipo", falha.TipoEvento);
            cmd.Parameters.Add("grav", falha.Gravidade);
            cmd.Parameters.Add("id", falha.Id);

            cmd.ExecuteNonQuery();
        }

        public void Remover(int id)
        {
            using var con = new OracleConnection(connectionString);
            con.Open();

            string sql = "DELETE FROM FALHA_ENERGIA WHERE ID = :id";
            using var cmd = new OracleCommand(sql, con);
            cmd.Parameters.Add("id", id);
            cmd.ExecuteNonQuery();
        }
    }
}

