using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

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
            this.Email = email;
            this.DataCriacao = DateTime.Now;
        }


        public void DefinirSenha(string senha)
        {
            if(senha.Length  < 8)
            {
                throw new("A senha precisa conter no minimo8 caracteres ! ");
            }
            else
            {
                this.Senha = senha;
            }
        }


    }
}
