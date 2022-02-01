using ContaCorrente.ApiConta.Data;
using ContaCorrente.ApiConta.Enums;
using ContaCorrente.ApiConta.Models;
using Microsoft.EntityFrameworkCore;

namespace ContaCorrente.ApiConta.Services
{
    public class UpdateSaldoService : IUpdateSaldoService
    {
        private readonly IConfiguration _configuration;
        private readonly ContaDataContext _context;

        public UpdateSaldoService(IConfiguration configuration)
        {

            _configuration = configuration;

            _context = new ContaDataContext(configuration);

        }
        public async Task UpdateSaldoAsync(Transacao transacao)
        {

            try
            {
                var conta = await _context
                            .Conta
                            .Where(x => x.Agencia == transacao.Agencia && x.Conta == transacao.Conta)
                            .FirstOrDefaultAsync();

                if (conta != null)
                {
                    conta.AtualizaSaldo((ETipoTransacao)transacao.TipoTransacao, transacao.Valor);

                    await this.RegisterUpdateSaldo(conta);
                }
            }
            catch (Exception)
            {

            }

        }

        private async Task RegisterUpdateSaldo(Contas conta)
        {
            _context.Conta.Update(conta);
            _context.SaveChanges();

        }
    }
}