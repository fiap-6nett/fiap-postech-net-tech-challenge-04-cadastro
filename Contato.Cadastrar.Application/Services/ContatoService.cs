using Contato.Cadastrar.Application.Dtos;
using Contato.Cadastrar.Application.Interfaces;
using Contato.Cadastrar.Domain.Entities;

namespace Contato.Cadastrar.Application.Services;

public class ContatoService : IContatoService
{
    private readonly IRabbitMqProducer _producer;
    
    public ContatoService(IRabbitMqProducer producer)
    {
        _producer = producer;
    }
    
    public ContatoEntity CadastrarContatoAsync(CadastrarContatoDto dto)
    {
        var id = Guid.NewGuid();
        
        var contato = new ContatoEntity();
         
        contato.SetId(id);
        contato.SetNome(dto.Nome);
        contato.SetTelefone(dto.Telefone);
        contato.SetEmail(dto.Email);
        contato.SetDdd(dto.Ddd);
        
        _producer.EnviarMensagem(contato);

        return  contato;
    }
}