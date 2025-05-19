namespace Core.Entity
{
    public  class Biblioteca:EntityBase
    {
        public int JogoID{ get; set; }
        public int UsuarioID{ get; set; }
        public Boolean JogoEmprestado{ get; set; }
        public DateTime DataAquisicao{ get; set; }

        public Jogo Jogo { get; set; }
        public  Usuario Usuario { get; set; }

        public Biblioteca()
        {
            DataAquisicao = DateTime.Now;
        }

    }
}
