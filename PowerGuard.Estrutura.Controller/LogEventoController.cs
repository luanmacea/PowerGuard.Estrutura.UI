using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGuard.Estrutura.Model;
using PowerGuard.Estrutura.Repository;

namespace PowerGuard.Estrutura.Controller
{
    public class LogEventoController
    {
        private readonly LogEventoRepository _repo = new();

        public void Registrar(LogEvento log)
        {
            log.DataHora = DateTime.Now;
            _repo.Inserir(log);
        }

        public List<LogEvento> Listar() => _repo.ListarTodos();
    }
}

