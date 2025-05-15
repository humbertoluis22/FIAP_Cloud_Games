namespace Core.Entity
{
    public  class Usuario : EntityBase
    {

        public string UserName { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public DateTime DataInscricao { get; set; }
        public int TentativasErradas { get; set; }
        public Boolean Bloqueado { get; set; }

        public Usuario()
        {
            
        }
        public Usuario(string userName, string senha)
        {
            this.UserName = userName;
            this.Senha = senha;
            this.DataInscricao = DateTime.Now;
        }

    }
}
