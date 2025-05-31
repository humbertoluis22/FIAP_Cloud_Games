namespace Core.Input.biblioteca
{
    public class BibliotecaJogoDTO
    {

        public int JogoId { get; set; }
        public string NomeJogo { get; set; }
        public string Genero { get; set; }
        public string Descricao { get; set; }
        public string Desenvolvedor { get; set; }
        public bool JogoEmprestado { get; set; }
        public bool EstaEmprestado { get; set; }
        public DateTime DataAquisicao { get; set; }
    }
}
