using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;

namespace ChainCrims.InputModels
{
    [Function("p_transfer", "bool")]
    public class TransferFunction : FunctionMessage
    {
        [Parameter("address", "_receiver", 1)]
        public string To { get; set; }

        [Parameter("uint256", "_amount", 2)]
        public BigInteger TokenAmount { get; set; }
    }

}
