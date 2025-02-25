namespace SisAt.Models
{
    public class CadastroDeHorarios
    {
        public int Id { get; set; }
        public int ServicoId { get; set; }
        public DateTime DataCadastrada { get; set; }
        public int HoraId { get; set; }
        public string? Hora { get; set; }
        public bool Marcado { get; set; }
    }
}
