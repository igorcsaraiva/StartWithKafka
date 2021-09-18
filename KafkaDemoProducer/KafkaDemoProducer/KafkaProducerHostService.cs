using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaDemoProducer
{
    /// <summary>
    /// Configuração do serviço responsável por enviar menssagens para o topico do kafka
    /// </summary>
    class KafkaProducerHostService : IHostedService
    {
        public KafkaProducerHostService(ILogger<KafkaProducerHostService> logger)
        {
            _logger = logger;
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092"
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        private readonly ILogger<KafkaProducerHostService> _logger;
        private readonly IProducer<Null, string> _producer;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Digite algo:");
            var value = Console.ReadLine();
            while (value is not "exit")
            {
                var response = await _producer.ProduceAsync("Hello_World", new Message<Null, string>()
                {
                    Value = value
                }, cancellationToken);

                _logger.LogInformation($"Response {response.Status}\n");
                _producer.Flush();

                Console.WriteLine("Digite algo:");
                value = Console.ReadLine();
            }           
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _producer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
