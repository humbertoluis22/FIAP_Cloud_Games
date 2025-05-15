using Microsoft.VisualBasic;

namespace Core.Entity
{
    public class Admin:EntityBase
    {
        public string Email { get; set; }
        public DateTime DataInscricao { get; set; }
        public string UserName { get; set; }
        public ICollection<Jogo> Jogos { get; set; }
        public string Senha 
        { 
            get { return Senha; }
            set
            {
                if (value.Length >= 8)
                {
                    Senha = value;
                }
                else
                {
                    throw new Exception("A senha deve ter no mínimo 8 caracteres");
                }
            }
        }
   

        public Admin()
        {
            
        }
        public Admin(string UserName,string Senha , string Email)
        {
            this.UserName = UserName;
            this.Senha = Senha;
            this.Email = Email;
            this.DataInscricao = DateTime.Now;
        }



    }
}
