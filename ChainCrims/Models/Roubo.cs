using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChainCrims.Models
{
    public partial class Roubo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string Nome { get; set; }
        public string Imagem { get; set; }
        public string Descriacao { get; set; }
        public float? ChanceBase { get; set; }
        public int? PoderMin { get; set; }
        public float? RecompensaMin { get; set; }
        public float? RecompensaMax { get; set; }
        public float? RespeitoMin { get; set; }
        public float? RespeitoMax { get; set; }
    }
}
