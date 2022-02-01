using ContaCorrente.ApiConta.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ContaCorrente.ApiConta.Services;

namespace ContaCorrente.ApiConta
{
    public class UpdateSaldo
    {
        private readonly IConfiguration _configuration;
        private readonly QueueClient _queueClient;
        private readonly IUpdateSaldoService _registerService;
        private const string QUEUE_SALDO = "saldo";

        public UpdateSaldo(IConfiguration configuration)
        {
            _configuration = configuration;

            _registerService = new UpdateSaldoService(_configuration);

            var connectionString = _configuration.GetSection("ServiceConnectionString").Value;

            _queueClient = new QueueClient(connectionString, QUEUE_SALDO);
        }

        public void RegisterHandler()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionHandler)
            {
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(ProcessMessageHandler, messageHandlerOptions);
        }

        private async Task ProcessMessageHandler(Message message, CancellationToken cancellationToken)
        {
            var messageString = Encoding.UTF8.GetString(message.Body);
            var transacaoModel = JsonConvert.DeserializeObject<Transacao>(messageString);

            await _registerService.UpdateSaldoAsync(transacaoModel);

            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);

        }

        private Task ExceptionHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            return Task.CompletedTask;
        }
    }
}