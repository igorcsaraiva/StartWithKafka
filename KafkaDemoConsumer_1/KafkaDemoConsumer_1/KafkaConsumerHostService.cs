using Kafka.Public;
using Kafka.Public.Loggers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaDemoConsumer
{
    /// <summary>
    /// Configuração do serviço responsável por receber menssagens do topico do kafka
    /// </summary>
    class KafkaConsumerHostService : IHostedService
    {
        private readonly ILogger<KafkaConsumerHostService> _logger;
        private readonly ClusterClient _cluster;

        public KafkaConsumerHostService(ILogger<KafkaConsumerHostService> logger)
        {
            _logger = logger;
            _cluster = new ClusterClient(new Configuration()
            {
                Seeds = "localhost:9092"

            }, new ConsoleLogger());

        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cluster.ConsumeFromEarliest("Hello_World");
            _cluster.MessageReceived += record =>
            {
                _logger.LogInformation($"Receivid -> {Encoding.UTF8.GetString(record.Value as byte[])}");
            };

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cluster?.Dispose();
            return Task.CompletedTask;
        }
    }
}
