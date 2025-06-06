using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGuard.Estrutura.Model
{
    public class FalhaEnergia
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public string Localizacao { get; set; } = string.Empty;
        public string CausaProvavel { get; set; } = string.Empty;
        public string TipoEvento { get; set; } = string.Empty;
        public string Gravidade { get; set; } = string.Empty;
    }

}
