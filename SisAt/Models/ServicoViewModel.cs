namespace SisAt.Models
{
    public class ServicoViewModel
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int AgendamentoId { get; set; }
        public DateTime DataDisponivel { get; set; }
    }
}
