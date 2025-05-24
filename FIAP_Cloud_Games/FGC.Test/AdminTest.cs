using Core.Entity;


namespace FGC.Test
{
    public class AdminTest
    {
        // tdd
        [Theory]
        [InlineData("Teste3124")]
        [InlineData("teste#124")]
        [InlineData("TesteTeste#")]
        public void DeveRetornarErroQuandoSenhaNaoAtenderAosRequisitos(string senha)
        {
            // arragen
            var admin = new Admin();

            //action
            var ex = Assert.Throws<Exception>(() => admin.DefinirSenha(senha));
           
            //assert
            Assert.Equal(
                "Senha precisa conter ao menos uma letra maiscula,um caracter especial e um digito!",
                ex.Message);
        }



        [Fact]
        public void DeveRetornarErroQuandoSenhaConterMenosDe8Caracteres()
        {
            //arrange
            var admin = new Admin();
            var senha = "Teste#1"; //senha com menos de 8 caracteres

            //action
            var ex = Assert.Throws<Exception>(() => admin.DefinirSenha(senha));

            //assert
            Assert.Equal("A senha precisa conter no minimo8 caracteres !", ex.Message);

        }


        [Theory]
        [InlineData("Teste#1234")]
        [InlineData("Valida@1")]
        [InlineData("SenhaBoa*2")]
        public void DeveCriarSenhaSeSenhaTiverNoFormatoValida(string senha)
        {
            //action
            var Admin = new Admin();
            Admin.DefinirSenha(senha);

            //assert	
            Assert.Equal(senha, Admin.Senha);
        }


        //tdd
        [Theory]
        [InlineData("Teste.com")]
        [InlineData("Teste@com")]
        public void DeveRetornarErroQuandoEmailNaoAtenderAosRequisitos(string email)
        {
            //arrange
            var admin = new Admin();
           
            //action
            var ex = Assert.Throws<Exception>(() => admin.DefinirEmail(email));
            
            //assert	
            Assert.Equal("Email invalido!", ex.Message);

        }



        [Theory]
        [InlineData("Teste@gmail.com")]
        [InlineData("Teste@by.com")]
        public void DeveCriarEmailQuandoEmailAtenderAosRequisitos(string email)
        {
            //arrange
            var admin = new Admin();

            //action
            admin.DefinirEmail(email);

            //assert	
            Assert.Equal(email, admin.Email);

        }



    }
}