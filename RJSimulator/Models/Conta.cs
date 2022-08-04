using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChainCrims.Models
{
    public partial class Conta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string Carteira { get; set; }
        public string HashSenha { get; set; }
        public double? Saldo { get; set; }
        public DateTime? UltimoSaque { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? Criacao { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UltimoLogin { get; set; }
        public bool? Banido { get; set; }
        [InverseProperty("Dono")]
        [ForeignKey("IdConta")]
        public virtual List<Arma> Armas { get; set; }
        [InverseProperty("Dono")]
        [ForeignKey("IdConta")]
        public virtual List<Suspeito> Suspeitos { get; set; }

        [ForeignKey("IdConta")]
        public virtual List<Registro> Registros { get; set; }

    }
}
