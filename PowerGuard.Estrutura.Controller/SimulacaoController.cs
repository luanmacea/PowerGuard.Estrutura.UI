using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGuard.Estrutura.Model;
using PowerGuard.Estrutura.Repository;

namespace PowerGuard.Estrutura.Controller
{
    public class SimulacaoController
    {
        private readonly SimulacaoRepository _repo = new();

        public void IniciarSimulacao(Simulacao sim)
        {
            if (string.IsNullOrWhiteSpace(sim.TipoSimulacao))
                throw new Exception("Tipo de simulação é obrigatório.");

            sim.DataHora = DateTime.Now;
            sim.Concluida = false;

            _repo.Inserir(sim);
        }

        public void ConcluirSimulacao(int id)
        {
            var sim = _repo.ObterPorId(id);
            sim.Concluida = true;
            _repo.Atualizar(sim);
        }

        public List<Simulacao> Listar() => _repo.ListarTodos();
    }
}

