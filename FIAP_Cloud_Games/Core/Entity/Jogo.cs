namespace Core.Entity
{
    public  class Jogo:EntityBase
    {
        public int IdAdmin { get; set; } 
        public string NomeJogo { get; set; }
        public string Genero{ get; set; }
        public DateTime DataCriacao { get; set; }
        public string Descricao { get; set; }
        public string  Desenvolvedor { get; set; }
        public decimal Preco { get; set; }

        public Admin Admin { get; set; }
        public ICollection<Biblioteca> Bibliotecas { get; set; }


        public Jogo()
        {

        }

        public Jogo(int idAdmin ,string nomeJogo, string genero, string descricao, string desenvolvedor, decimal preco)
        {
            this.DataCriacao = DateTime.Now;
            this.IdAdmin = idAdmin;
            this.NomeJogo = nomeJogo;
            this.Genero = genero;
            this.Descricao = descricao;
            this.Desenvolvedor = desenvolvedor;
            this.Preco = preco;
        }


        public void AplicarPorcentagemDesconto(double quantidade_desconto)
        {
            if (quantidade_desconto < 0.5)
            {
                throw new("Porcentagem de desconto tem que ser superior a 5");
            }

            var novoValor = this.Preco - (this.Preco * (decimal)quantidade_desconto / 100);
            
            this.Preco = novoValor;
        }

    }
}
