namespace Core.Entity
{
    public  class Jogo
    {
        public int IdAdmin { get; set; } 
        public string NomeJogo { get; set; }
        public string Genero{ get; set; }
        public DateTime DataCriacao{ get; set; }
        public string Descricao { get; set; }
        public string  Desenvolvedor { get; set; }
        public decimal Preco { get; set; }

        public Admin Admin { get; set; }

        public Jogo()
        {
            
        }

        public Jogo(int idAdmin ,string nomeJogo, string genero, string descricao, string desenvolvedor, decimal preco)
        {
            IdAdmin = idAdmin;
            NomeJogo = nomeJogo;
            Genero = genero;
            DataCriacao = DateTime.Now;
            Descricao = descricao;
            Desenvolvedor = desenvolvedor;
            Preco = preco;
        }


    }
}
