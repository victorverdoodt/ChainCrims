namespace ChainCrims.Models
{
    public class ResultadoRoubo
    {
        public int? Id { get; set; }
        public int? IdSuspeito { get; set; }
        public int? IdRoubo { get; set; }
        public string Resultado { get; set; }
        public double? Respeito { get; set; }
        public double? Saldo { get; set; }
        virtual public Roubo Roubo { get; set; }
        virtual public Suspeito Suspeito { get; set; }
    }
}
