namespace Contato.Cadastrar.Application.Interfaces;

public interface IRabbitMqProducer
{
    void EnviarMensagem(object mensagem);
}