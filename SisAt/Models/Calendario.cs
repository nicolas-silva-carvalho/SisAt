namespace SisAt.Models
{
    public class Calendario
    {
        public int Id { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public bool allDay { get; set; }
        public int AgendamentoId { get; set; }
        public virtual Agendamento Agendamento { get; set; }
    }
}
