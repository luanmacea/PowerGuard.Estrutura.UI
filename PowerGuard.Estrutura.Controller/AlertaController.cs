using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGuard.Estrutura.Model;
using PowerGuard.Estrutura.Repository;

namespace PowerGuard.Estrutura.Controller
{
    public class AlertaController
    {
        private readonly AlertaRepository _repo = new();

        public void Emitir(Alerta alerta)
        {
            if (string.IsNullOrWhiteSpace(alerta.Mensagem))
                throw new Exception("Mensagem do alerta é obrigatória.");

            alerta.DataHora = DateTime.Now;
            _repo.Inserir(alerta);
        }

        public List<Alerta> Listar() => _repo.ListarTodos();

        public void Remover(int id) => _repo.Remover(id);
    }
}

