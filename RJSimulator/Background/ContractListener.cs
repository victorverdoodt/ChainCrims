using ChainCrims.DataBase;
using ChainCrims.Enums;
using ChainCrims.InputModels;
using ChainCrims.Models;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainCrims.Background
{
    public class ContractListener : BackgroundService
    {
        public IServiceProvider Services { get; }
        public IConfiguration Configuration { get; }
        public ITopicEventSender EventSender { get; }
        public ContractListener(IServiceProvider services, IConfiguration configuration, ITopicEventSender eventSender)
        {
            Services = services;
            EventSender = eventSender;
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
            string valueBase = "{\"query\":\"{ethereum(network: " + rede + ") { smartContractEvents(options: {asc: \\\"block.height\\\", limit: " + limite + "} height: {gt: " + bloco + "} smartContractAddress: {is: \\\"" + carteira + "\\\"}) {block {height  timestamp { unixtime }} transaction { hash } eventIndex arguments { value argument index argumentType } smartContractEvent { name signature signatureHash } date { date } } }}\",\"variables\":{}}";
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
                var Carteira = Configuration["Apps:NFT:Contrato"];
                request.AddParameter("application/json", GenerateQuery(Rede, 100, LastBlock, Carteira), ParameterType.RequestBody);
                var response = client.Execute(request);
                var walletResponse = ContractResponse.FromJson(response.Content);

                foreach (var teste in walletResponse.Data.Ethereum.SmartContractEvents)
                {
                    if (teste.SmartContractEvent.Name == "Birth")
                    {
                        if (teste.Arguments[0].ArgumentArgument == "owner" && !teste.Arguments[0].Value.Contains("0x0000000"))
                        {
                            var newTran = new Transacao { Amount = 1, BlockHeight = teste.Block.Height, BlockTime = teste.Block.Timestamp.Time.DateTime, DataCriacao = DateTime.Now, Receiver = teste.Arguments[0].Value, Sender = "0x0000000000000000000000000000000000000000", Symbol = "ERC721", Tx = teste.Transaction.Hash, Type = TransacaoTipo.Birth, Status = true };
                            var find = await context.Transacoes.FirstOrDefaultAsync(x => x.Tx == teste.Transaction.Hash && x.Status == true);
                            if (find == null)
                            {
                                await context.Transacoes.AddAsync(newTran);
                                var findConta = await context.Contas.FirstOrDefaultAsync(x => x.Carteira.ToLower() == teste.Arguments[0].Value.ToLower());
                                if (findConta != null)
                                {
                                    var suspct = new Suspeito { Criacao = DateTime.Now, IdConta = findConta.Id, IdBlockchain = int.Parse(teste.Arguments[1].Value), Nivel = 1, Banido = false, Nome = "Victor", Saude = 1000, Estamina = 4, Poder = 500, Titulo = "Vida louca", ProximaRecarga = DateTime.Now.AddHours(12), Imagem = "caule.png", Queimado = false };
                                    await context.Suspeitos.AddAsync(suspct);
                                    await EventSender.SendAsync("Birth", suspct);
                                    Console.WriteLine(teste.SmartContractEvent.Name + ": { " + teste.Transaction.Hash + " }");
                                }
                                await context.SaveChangesAsync();
                            }

                        }
                    }
                    else if (teste.SmartContractEvent.Name == "Transfer")
                    {
                        if (teste.Arguments[0].ArgumentArgument == "from" && !teste.Arguments[0].Value.Contains("0x0000000"))
                        {
                            var newTran = new Transacao { Amount = 1, BlockHeight = teste.Block.Height, BlockTime = teste.Block.Timestamp.Time.DateTime, DataCriacao = DateTime.Now, Receiver = teste.Arguments[0].Value, Sender = teste.Arguments[1].Value, Symbol = "ERC721", Tx = teste.Transaction.Hash, Type = TransacaoTipo.Transfer, Status = true };
                            var find = await context.Transacoes.FirstOrDefaultAsync(x => x.Tx == teste.Transaction.Hash && x.Status == true);
                            if (find == null)
                            {
                                await context.Transacoes.AddAsync(newTran);
                                var findConta = await context.Contas.FirstOrDefaultAsync(x => x.Carteira.ToLower() == teste.Arguments[0].Value.ToLower());
                                if (findConta != null)
                                {
                                    var findNewOwner = await context.Contas.FirstOrDefaultAsync(x => x.Carteira.ToLower() == teste.Arguments[1].Value.ToLower());
                                    if (findNewOwner != null)
                                    {
                                        var findSuspect = await context.Suspeitos.FirstOrDefaultAsync(x => x.IdConta == findConta.Id && x.IdBlockchain == int.Parse(teste.Arguments[2].Value));
                                        findSuspect.IdConta = findNewOwner.Id;
                                        context.Update(findSuspect);
                                        Console.WriteLine(teste.SmartContractEvent.Name + ": { " + teste.Transaction.Hash + " }");
                                    }
                                }
                                await context.SaveChangesAsync();
                            }
                            break;
                        }
                    }
                }
                await context.SaveChangesAsync();
            }
            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }
}
