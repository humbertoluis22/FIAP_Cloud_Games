using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace Core.Entity
{
    public class Admin:EntityBase
    {
        public string Email { get; set; }
        public DateTime DataCriacao { get; set; }
        public string UserName { get; set; }
        public ICollection<Jogo> Jogos { get; set; }

        public string Senha { get; set; } 



        public Admin()
        {
            
        }
        public Admin(string userName,string senha , string email)
        {
            this.UserName = userName;
            DefinirSenha(senha);
            DefinirEmail(email);
            this.DataCriacao = DateTime.Now;
        }


        public void DefinirSenha(string senha)
        {
            if(senha.Length  < 8)
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


    }
}
