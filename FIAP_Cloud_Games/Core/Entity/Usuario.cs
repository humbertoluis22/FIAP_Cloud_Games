using System.Text.RegularExpressions;

namespace Core.Entity
{
    public class Usuario : EntityBase
    {

        public string UserName { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public DateTime DataInscricao { get; set; }
        public int TentativasErradas { get; set; }
        public Boolean Bloqueado { get; set; }

        public ICollection<Biblioteca> Bibliotecas { get; set; }

        public Usuario()
        {

        }
        public Usuario(string userName, string senha, string email)
        {
            this.UserName = userName;
            DefinirSenha(senha);
            DefinirEmail(email);
            this.DataInscricao = DateTime.Now;
            this.Bloqueado = false;
        }


        // apenas um esboço melhorar logica da senha
        public void AlterarSenha(string senha)
        {
            if (senha.Length < 8)
            {
                throw new("A senha precisa conter no minimo8 caracteres ! ");
            }
            else
            {
                DefinirSenha(senha);
            }
        }


        public void AdicionarTentativaErrada()
        {
            if (TentativasErradas >= 3)
            {
                Bloqueado = true;
                throw new("Usuario bloqueado, entre em contato com o Admin");
            }
            TentativasErradas += 1;
           
        }

        public void ZerarTentativasErrada()
        {
            TentativasErradas = 0;
        }


        public void DefinirSenha(string senha)
        {
            if (senha.Length < 8)
            {
                throw new("A senha precisa conter no minimo8 caracteres !");
            }
            // valida se senha tem ao menos
            // uma letra maiscula, um caracter especial e um digito
            var padrao_senha = "^(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*(),.?\":{}|<>]).+$";

            bool math = Regex.IsMatch(senha, padrao_senha);
            if (!math)
            {
                throw new("Senha precisa conter ao menos uma letra maiscula,um caracter especial e um digito!");
            }
            this.Senha = senha;
        }



        public void DefinirEmail(string email)
        {
            var padrao_email = "^[\\w\\.-]+@[\\w\\-]+\\.\\w{2,}$";

            bool math = Regex.IsMatch(email, padrao_email);
            if (!math)
            {
                throw new("Email invalido!");
            }

            this.Email = email;
        }


        public void BloquearUsuario()
        {
            if (Bloqueado)
            {
                throw new("Usuario ja bloqueado");
            }
            this.Bloqueado = true;
        }


        public void DesbloquearUsuario()
        {
            if (!Bloqueado)
            {
                throw new("Usuario ja desbloqueado");
            }
            this.Bloqueado = false;
        }



    }
}
