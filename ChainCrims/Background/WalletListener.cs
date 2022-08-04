using ChainCrims.DataBase;
using ChainCrims.Enums;
using ChainCrims.InputModels;
using ChainCrims.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nethereum.Contracts;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using RestSharp;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainCrims.Background
{
    public class WalletListener : BackgroundService
    {
        public IServiceProvider Services { get; }
        public IConfiguration Configuration { get; }
        public WalletListener(IServiceProvider services, IConfiguration configuration)
        {
            Services = services;
            Configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork();
            }
        }

        private string GenerateQuery(string rede, int limite, int bloco, string carteira)
        {
            string valueBase = "{\"query\":\"{ethereum(network: " + rede + ") { transfers(options: {asc: \\\"block.height\\\",  limit: " + limite + "} date: {since: null, till: null} amount: {is: 0.0009} height: {gt: " + bloco + "} receiver: {is: \\\"" + carteira + "\\\"}    ) { block { timestamp { time(format: \\\"%Y-%m-%d %H:%M:%S\\\") }  height     }  addressTo: receiver {  address  }  addressFrom: sender{  address }  currency {   address  symbol  name }  amount     amountInUSD: amount(in: USD) transaction {       hash    }   }  }}\",\"variables\":{}}";
            return valueBase;
        }

        private async Task DoWork()
        {
            var client = new RestClient("https://graphql.bitquery.io")
            {
                Timeout = -1
            };

            using (var scope = Services.CreateScope())
            {
                var context =
                    scope.ServiceProvider
                        .GetRequiredService<ChainCrimsContext>();

                var lastTran = context.Transacoes.Where(x => x.Status == true).OrderByDescending(x => x.BlockHeight).FirstOrDefault();
                var request = new RestRequest(Method.POST);
                int LastBlock = 0;
                var Chave = Configuration["Apps:BitQuery:Chave"];
                request.AddHeader("X-API-KEY", Chave);
                request.AddHeader("Content-Type", "application/json");
                if (lastTran != null)
                {
                    LastBlock = (int)lastTran.BlockHeight;
                }

                var Rede = Configuration["Apps:BitQuery:Rede"];
                var Carteira = Configuration["Apps:BitQuery:Carteira"];
                request.AddParameter("application/json", GenerateQuery(Rede, 100, LastBlock, Carteira), ParameterType.RequestBody);
                var response = client.Execute(request);
                var walletResponse = WalletResponse.FromJson(response.Content);

                foreach (var teste in walletResponse.Data.Ethereum.Transfers)
                {
                    var newTran = new Transacao { Amount = teste.Amount, BlockHeight = teste.Block.Height, BlockTime = teste.Block.Timestamp.Time.DateTime, DataCriacao = DateTime.Now, Receiver = teste.AddressTo.addressBase, Sender = teste.AddressFrom.addressBase, Symbol = teste.Currency.Symbol, Tx = teste.Transaction.Hash, Type = TransacaoTipo.Transfer, Status = true };
                    var find = await context.Transacoes.FirstOrDefaultAsync(x => x.Tx == teste.Transaction.Hash);
                    if (find == null)
                    {
                        await context.Transacoes.AddAsync(newTran);
                        if (teste.Amount == 0.0009)
                        {
                            Console.WriteLine("Fazer deposito: " + teste.Transaction.Hash);
                            var acharConta = await context.Contas.FirstOrDefaultAsync(x => x.Carteira.ToLower() == teste.AddressFrom.addressBase.ToLower());
                            if (acharConta != null)
                            {
                                var account = new Account(Configuration["Apps:Rede:Carteira"], int.Parse(Configuration["Apps:Rede:ChainId"]));
                                var RPC = new Web3(account, Configuration["Apps:Rede:RPC"]);
                                RPC.TransactionManager.UseLegacyAsDefault = true;
                                var balance = await RPC.Eth.GetBalance.SendRequestAsync(account.Address);
                                Console.WriteLine(balance);
                                var transactionMessage = new TransferFunction
                                {
                                    To = acharConta.Carteira.ToLower(),

                                    TokenAmount = Web3.Convert.ToWei((Double)acharConta.Saldo)
                                };

                                var transferHandler = RPC.Eth.GetContractTransactionHandler<TransferFunction>();
                                var transferReceipt = await transferHandler.SendRequestAndWaitForReceiptAsync(Configuration["Apps:Moeda:Contrato"], transactionMessage);
                                var transaction = await RPC.Eth.Transactions.GetTransactionByHash.SendRequestAsync(transferReceipt.TransactionHash);
                                if (!(bool)transferReceipt.HasErrors())
                                {
                                    var transferDecoded = transaction.DecodeTransactionToFunctionMessage<TransferFunction>();
                                    acharConta.Saldo -= acharConta.Saldo;
                                    acharConta.UltimoSaque = DateTime.Now;
                                    context.Update(acharConta);
                                    await context.SaveChangesAsync();
                                    Console.WriteLine("WithDraw: { " + transaction.TransactionHash + " }");
                                }
                            }
                        }
                    }
                }
                await context.SaveChangesAsync();
            }
            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }
}
