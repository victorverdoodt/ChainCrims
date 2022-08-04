using BscScanner;
using BscScanner.Data;
using ChainCrims.DataBase;
using ChainCrims.Models;
using HotChocolate;
using HotChocolate.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChainCrims.Data
{
    public class Query
    {
        /// <summary>
        /// Retorna todas as contas do banco.
        /// </summary>

        [UseFiltering]
        [UseSorting]
        public IQueryable<Conta> GetContas([Service] ChainCrimsContext contasContext) =>
            contasContext.Contas;

        [UseFiltering]
        [UseSorting]
        public IQueryable<Transacao> GetTransacion([Service] ChainCrimsContext contasContext) =>
            contasContext.Transacoes;

        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<BscTransaction>> GetTransactions([Service] IBscScanClient bscContext)
        {

            //var teste = await bscContext.GetTransactionsByAddress("0xf91844f088182ad6b90b818d639dd8015c3c8bd5");
            var teste = await bscContext.GetTransactionsByBlockRange();

            return teste.Where(x => x.To == "0xe99aa27e7ac5efcb48be018a344cd049e9b12dd1" && x.Value == "900000000000000");
        }
    }
}