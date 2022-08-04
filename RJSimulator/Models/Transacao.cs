using ChainCrims.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChainCrims.Models
{
    public class Transacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string Tx { get; set; }
        public double? Amount { get; set; }
        public TransacaoTipo? Type { get; set; }
        public string Symbol { get; set; }
        public string Receiver { get; set; }
        public string Sender { get; set; }
        public long? BlockHeight { get; set; }
        public DateTime BlockTime { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool? Status { get; set; }

    }
}
