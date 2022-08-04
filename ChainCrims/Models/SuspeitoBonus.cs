using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChainCrims.Models
{
    public partial class SuspeitoBonus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public int? IdBonus { get; set; }
        public int? Duracao { get; set; }
        public DateTime? Criacao { get; set; }
    }
}
