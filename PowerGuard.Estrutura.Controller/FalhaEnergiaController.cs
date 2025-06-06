using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGuard.Estrutura.Model;
using PowerGuard.Estrutura.Repository;

namespace PowerGuard.Estrutura.Controller
{
    public class FalhaEnergiaController
    {
        private readonly FalhaEnergiaRepository _repo = new();

        public void Cadastrar(FalhaEnergia falha)
        {
            if (string.IsNullOrWhiteSpace(falha.Localizacao))
                throw new Exception("Localização é obrigatória.");

            if (falha.DataHora == default)
                throw new Exception("Data e hora inválidas.");

            _repo.Inserir(falha);
        }

        public List<FalhaEnergia> Listar() => _repo.ListarTodos();

        public void Atualizar(FalhaEnergia falha) => _repo.Atualizar(falha);

        public void Remover(int id) => _repo.Remover(id);
    }
}
