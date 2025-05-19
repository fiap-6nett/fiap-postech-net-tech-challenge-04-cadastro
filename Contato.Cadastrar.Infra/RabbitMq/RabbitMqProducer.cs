using System.Text;
using System.Text.Json;
using Contato.Cadastrar.Application.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;


namespace Contato.Cadastrar.Infra.RabbitMq;

public class RabbitMqProducer : IRabbitMqProducer
{
    private readonly RabbitMQSettings _settings;

    public RabbitMqProducer(IOptions<RabbitMQSettings> options)
    {
        _settings = options.Value;
    }
    
    public void EnviarMensagem(object mensagem)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            UserName = _settings.Username,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        
        channel.QueueDeclare(
            queue: _settings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var json = JsonSerializer.Serialize(mensagem);
        var body = Encoding.UTF8.GetBytes(json);
        
        channel.BasicPublish(
            exchange: "",
            routingKey: _settings.QueueName,
            basicProperties: null,
            body: body
        );
    }
}