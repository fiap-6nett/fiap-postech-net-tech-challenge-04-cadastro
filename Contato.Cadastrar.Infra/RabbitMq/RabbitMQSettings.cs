namespace Contato.Cadastrar.Infra.RabbitMq;

public class RabbitMQSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = "/";
    public string QueueName { get; set; } = string.Empty;
}