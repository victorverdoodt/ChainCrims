using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChainCrims.Models
{
    public partial class Suspeito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public int? IdConta { get; set; }
        public int? IdBlockchain { get; set; }
        public int? IdArma { get; set; }
        public string Imagem { get; set; }
        public string Nome { get; set; }
        public string Titulo { get; set; }
        public int? Estamina { get; set; }
        public double? Respeito { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Nivel { get; set; } = 1;
        public float? Poder { get; set; }
        public float? Saude { get; set; }
        public List<int> Habilidades { get; set; } //Depois mudar para um Enum
        public bool? Banido { get; set; }
        public bool? Queimado { get; set; }
        public DateTime? Criacao { get; set; }
        public DateTime? UltimaLuta { get; set; }
        public DateTime? ProximaRecarga { get; set; }
        [ForeignKey("IdArma")]
        virtual public Arma Arma { get; set; }
        virtual public Conta Dono { get; set; }
    }
}
