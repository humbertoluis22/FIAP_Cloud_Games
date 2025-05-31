using System.Reflection;

namespace Core.Input
{
    public class JogoUpdateInput
    {
        public int JogoID { get; set; }
        public string NomeJogo { get; set; }
        public string Genero { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
    }
}
