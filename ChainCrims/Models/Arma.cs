using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChainCrims.Models
{
    public partial class Arma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public int? IdConta { get; set; }
        public int? IdSuspeito { get; set; }
        public int? IdBlockchain { get; set; }
        public string Imagem { get; set; }
        public string Nome { get; set; }
        public float? Poder { get; set; }
        public int? Saude { get; set; }
        public bool? Banido { get; set; }
        public bool? Queimado { get; set; }
        public DateTime? Criacao { get; set; }
        virtual public Suspeito Suspeito { get; set; }
        virtual public Conta Dono { get; set; }
    }
}
