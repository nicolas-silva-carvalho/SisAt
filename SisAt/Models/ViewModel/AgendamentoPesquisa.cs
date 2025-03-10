namespace SisAt.Models.ViewModel
{
    public class AgendamentoPesquisa
    {
        public int Id { get; set; }
        public string CpfCnpj { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string ServicoNome { get; set; }
        public DateTime DataMarcada { get; set; }
        public string Hora { get; set; }
        public string? Protocolo { get; set; }
    }
}
