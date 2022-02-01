using ContaCorrente.ApiConta.Models;
using System.Threading.Tasks;

namespace ContaCorrente.ApiConta.Services
{
    public interface IUpdateSaldoService
    {
        Task UpdateSaldoAsync(Transacao transacao);
    }
}