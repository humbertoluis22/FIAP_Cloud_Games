namespace Core.Entity
{
    public  class Biblioteca:EntityBase
    {
        public int JogoID{ get; set; }
        public int UsuarioID{ get; set; }
        public Boolean JogoEmprestado{ get; set; }
        public Boolean EstaEmprestado{ get; set; }
        public DateTime DataAquisicao{ get; set; }

        public Jogo Jogo { get; set; }
        public  Usuario Usuario { get; set; }

        public Biblioteca()
        {
            DataAquisicao = DateTime.Now;
            JogoEmprestado = false;
            EstaEmprestado = false;
        }


        public void EmprestarJogo()
        {
            if (EstaEmprestado)
            {
                throw new Exception("O jogo já está emprestado.");
            }
            EstaEmprestado = true;
        }


        public void DevolverJogo()
        {
            if (!JogoEmprestado)
            {
                throw new Exception("O jogo não é emprestado.");
            }
            JogoEmprestado = false;
        }


        public void SolicitarJogo()
        {
            if (!EstaEmprestado)
            {
                throw new Exception("O jogo não está emprestado.");
            }
            EstaEmprestado = false;

        }


    }
}
