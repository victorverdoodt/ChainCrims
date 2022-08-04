using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChainCrims.Models
{
    public partial class Registro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public int? IdConta { get; set; }
        public int? Acao { get; set; }
        public virtual List<RegistroArma> RegistroArmas { get; set; }
        public virtual List<RegistroSuspeito> RegistroSuspeitos { get; set; }
        public DateTime? Criacao { get; set; }
    }
}
