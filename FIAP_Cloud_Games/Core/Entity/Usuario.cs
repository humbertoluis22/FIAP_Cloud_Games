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
            this.Senha = senha;
            this.Email = email;
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
                this.Senha = senha;
            }
        }
    }
}
