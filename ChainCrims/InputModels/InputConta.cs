using System;

namespace ChainCrims.InputModels
{
    public class InputConta
    {
        public string Carteira { get; set; }
        public string HashSenha { get; set; }
        public float? Saldo { get; set; }
        public float? SaldoBloqueado { get; set; }
        public DateTime? UltimoSaque { get; set; }
        public DateTime? Criacao { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public bool? Banido { get; set; }
    }
}
