using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChainCrims.Models
{
    public partial class Bonus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string Nome { get; set; }
        public string Descriacao { get; set; }
        public string ValorBonus { get; set; }
        public int? DuracaoMax { get; set; }
    }
}
