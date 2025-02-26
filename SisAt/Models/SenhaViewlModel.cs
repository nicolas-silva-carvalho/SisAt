namespace SisAt.Models
{
    public class SenhaViewlModel
    {
        public int Numero { get; set; }
        public int Id_servico { get; set; }
        public int Prioridade { get; set; }
        public int Status { get; set; }
        public string Cidadao { get; set; }
        public string Complemento { get; set; }
        public DateTime Horario { get; set; }
    }
}
