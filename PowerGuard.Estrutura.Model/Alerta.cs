using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGuard.Estrutura.Model
{
    public class Alerta
    {
        public int Id { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
        public int UsuarioId { get; set; }
        public int? FalhaEnergiaId { get; set; }
    }
}
