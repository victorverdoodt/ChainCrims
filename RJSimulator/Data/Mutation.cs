using ChainCrims.DataBase;
using ChainCrims.Models;
using HotChocolate;
using HotChocolate.Subscriptions;
using System;
using System.Threading.Tasks;

namespace ChainCrims.Data
{
    public class Mutation
    {
        public async Task<Conta> CriarConta([Service] ChainCrimsContext _context, [Service] ITopicEventSender eventSender, Conta model)
        {
            var createdConta = await _context.Contas.AddAsync(model);
            await _context.SaveChangesAsync();

            await eventSender.SendAsync("DepartmentCreated", createdConta);

            return createdConta.Entity;
        }



        public async Task<ResultadoRoubo> Batalhar([Service] ChainCrimsContext _context, [Service] ITopicEventSender eventSender, int idRoubo, int idSuspeito)
        {
            var LocalRoubo = await _context.Roubos.FindAsync(idRoubo);
            var SuspeitoAtual = await _context.Suspeitos.FindAsync(idSuspeito);
            Random rnd = new Random();
            double? expGain = 0;
            double? saldoGain = 0;
            string ResultadoTxt = "Perdeu";
            if (SuspeitoAtual.Poder + SuspeitoAtual.Arma.Poder > LocalRoubo.PoderMin && SuspeitoAtual.Estamina > 0)
            {
                var Rng = rnd.Next(0, 100);
                var CalcChance = LocalRoubo.ChanceBase + (SuspeitoAtual.Nivel * 2);
                if (Rng <= CalcChance)
                {
                    expGain = rnd.NextDouble() * (LocalRoubo.RespeitoMax - LocalRoubo.RespeitoMin) + LocalRoubo.RespeitoMin;
                    saldoGain = rnd.NextDouble() * (LocalRoubo.RecompensaMax - LocalRoubo.RecompensaMin) + LocalRoubo.RecompensaMin;
                    SuspeitoAtual.Respeito += expGain;
                    SuspeitoAtual.Dono.Saldo += saldoGain;
                    SuspeitoAtual.Estamina -= 1;
                    SuspeitoAtual.UltimaLuta = DateTime.Now;
                    if (SuspeitoAtual.Respeito >= (SuspeitoAtual.Nivel * 100))
                    {
                        SuspeitoAtual.Respeito = 0;
                        SuspeitoAtual.Nivel += 1;
                    }
                    ResultadoTxt = "Ganhou";
                    _context.Update(SuspeitoAtual);
                }
            }

            var Resultado = new ResultadoRoubo { IdRoubo = idRoubo, IdSuspeito = SuspeitoAtual.Id, Respeito = expGain, Resultado = ResultadoTxt, Saldo = saldoGain };
            await _context.ResultadoRoubos.AddAsync(Resultado);
            await _context.SaveChangesAsync();
            await eventSender.SendAsync("Roubo", Resultado);
            return Resultado;
        }
    }
}
