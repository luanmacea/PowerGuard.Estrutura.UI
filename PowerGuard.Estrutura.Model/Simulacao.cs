using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGuard.Estrutura.Model
{
    public class Simulacao
    {
        public int Id { get; set; }
        public string TipoSimulacao { get; set; } = string.Empty; // Ex: "Ataque Ransomware", "Apagão Geral"
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
        public bool Concluida { get; set; }
    }
}
