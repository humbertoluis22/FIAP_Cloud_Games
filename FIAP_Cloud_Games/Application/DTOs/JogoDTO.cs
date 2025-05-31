using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Input.jogo
{
    public class JogoDto
    {
        public int JogoId { get; set; }
        public string NomeJogo { get; set; }
        public string Genero { get; set; }
        public string Descricao { get; set; }
        public string Desenvolvedor { get; set; }
        public decimal Preco { get; set; }
    }
}
