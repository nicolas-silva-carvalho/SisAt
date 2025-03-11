namespace SisAt.Models.ViewModel
{
    public class CalendarioViewlModel
    {
        public int Id { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public bool allDay { get; set; }
        public int AgendamentoId { get; set; }
    }
}
