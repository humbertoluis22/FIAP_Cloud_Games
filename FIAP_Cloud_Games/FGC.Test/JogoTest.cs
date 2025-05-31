using Core.Entity;

namespace FGC.Test
{
    public  class JogoTest
    {
        //tdd
        [Fact]
        public void AplicarPorcentagemDesconto_DeveRetornarValorComDesconto_QuandoDescontoAplicado()
        {
            //arrange
            var jogo = new Jogo();
            jogo.Preco = 100;
            var quantidadeDesconto = 10.0;

            var valorEsperado = jogo.Preco - (jogo.Preco * (decimal)quantidadeDesconto/100 ); 

            //action
            jogo.AplicarPorcentagemDesconto(quantidadeDesconto);
  
            //assert	
            Assert.Equal(valorEsperado, jogo.Preco);

        }


        [Fact]
        public void AplicarPorcentagemDesconto_DeveRetornarErro_QuandoPorcentagemDescontoForMenorQue5()
        {
            //arrange
            var jogo = new Jogo();
            jogo.Preco = 100;
            var quantidadeDesconto = 0.4;

            var valorEsperado = jogo.Preco - (jogo.Preco * (decimal)quantidadeDesconto / 100);

            //action
            var ex = Assert.Throws<Exception>(() => 
                jogo.AplicarPorcentagemDesconto(quantidadeDesconto));

            //assert	
            Assert.Equal("Porcentagem de desconto tem que ser superior a 5",ex.Message);

        }
    }
}
