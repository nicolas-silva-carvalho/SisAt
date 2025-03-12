namespace SisAt.API.ModelsAPI
{
    public class ServicosDadosApi
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int inicio { get; set; }
        public string sigla { get; set; }
        public int id_unidade { get; set; }
        public int senha_inicio { get; set; }
        public int senha_fim { get; set; }
        public int senha_atual { get; set; }
    }
}
