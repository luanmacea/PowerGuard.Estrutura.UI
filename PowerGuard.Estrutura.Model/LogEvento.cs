using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGuard.Estrutura.Model
{
    public class LogEvento
    {
        public int Id { get; set; }
        public string Acao { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
        public int UsuarioId { get; set; }
    }

}
