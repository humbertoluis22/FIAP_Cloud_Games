using Core.Entity;

namespace FGC.Test
{
    public class UsuarioTest
    {
        //tdd
       [Theory]
       [InlineData("Teste12111")]
       [InlineData("teste#1222")]
       [InlineData("TesteTe#")]
        public void DeveRetornarErroQuandoSenhaNaoAtenderAosRequisitos(string senha)
        {
            // arragen
            var usuario = new Usuario();

            //action
            var ex = Assert.Throws<Exception>(() => usuario.DefinirSenha(senha));

            //assert
            Assert.Equal(
                "Senha precisa conter ao menos uma letra maiscula,um caracter especial e um digito!",
                ex.Message);
        }


        //tdd
        [Fact]
        public void DeveRetornarErroQuandoSenhaConterMenosDe8Caracteres()
        {
            //arrange
            var usuario = new Usuario();
            var senha = "Teste#1"; //senha com menos de 8 caracteres

            //action
            var ex = Assert.Throws<Exception>(() => usuario.DefinirSenha(senha));

            //assert
            Assert.Equal("A senha precisa conter no minimo8 caracteres !", ex.Message);

        }


        [Theory]
        [InlineData("OutraSenha#1234")]
        [InlineData("Validado@1")]
        [InlineData("MuitoBoa*2")]
        public void DeveCriarSenhaSeSenhaTiverNoFormatoValida(string senha)
        {
            //action
            var usuario = new Usuario();
            usuario.DefinirSenha(senha);

            //assert	
            Assert.Equal(senha, usuario.Senha);
        }



        //tdd
        [Theory]
        [InlineData("usuario.com")]
        [InlineData("Usuario@com")]
        public void DeveRetornarErroQuandoEmailNaoAtenderAosRequisitos(string email)
        {
            //arrange
            var usuario = new Usuario();

            //action
            var ex = Assert.Throws<Exception>(() => usuario.DefinirEmail(email));

            //assert	
            Assert.Equal("Email invalido!", ex.Message);

        }



        [Theory]
        [InlineData("usuario@gmail.com")]
        [InlineData("usuario@hotmail.com")]
        public void DeveCriarEmailQuandoEmailAtenderAosRequisitos(string email)
        {
            //arrange
            var usuario = new Usuario();

            //action
            usuario.DefinirEmail(email);

            //assert	
            Assert.Equal(email, usuario.Email);

        }


        //tdd
        [Fact]
        public void DeveBloquearUsuarioQuandoSolicitado()
        {
            //arrange
            var usuario = new Usuario();

            //action
            usuario.BloquearUsuario();

            //assert
            Assert.Equal(true, usuario.Bloqueado);
            Assert.True(usuario.Bloqueado);
        }


        [Fact]
        public void DeveRetornarErroQuandoUsuarioJaBloqueado()
        {
            //arrange
            var usuario = new Usuario();
            usuario.Bloqueado = true;
            //action
            var ex = Assert.Throws<Exception>(() => usuario.BloquearUsuario());
            //assert
            Assert.Equal("Usuario ja bloqueado", ex.Message);
        }


        //tdd
        [Fact]
        public void DeveDesbloquearUsuarioQuandoBloqueado()
        {
            //arrange
            var usuario = new Usuario();
            usuario.Bloqueado = true;
            //action
            usuario.DesbloquearUsuario();

            //assert
            Assert.Equal(false, usuario.Bloqueado);
            Assert.False(usuario.Bloqueado);
        }


        [Fact]
        public void DeveRetornarErroQuandoTentaDesbloquarUsuarioDesbloqueado()
        {
            //arrange
            var usuario = new Usuario();
            usuario.Bloqueado = false;
            //action
            var ex = Assert.Throws<Exception>(() => usuario.DesbloquearUsuario());
            //assert
            Assert.Equal("Usuario ja desbloqueado", ex.Message);
        }


    }
}
