using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChainCrims.Models
{
    public partial class RegistroSuspeito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public int? IdSuspeito { get; set; }
        public int? IdContaPara { get; set; }
    }
}
