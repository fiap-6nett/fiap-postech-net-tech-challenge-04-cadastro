using Contato.Cadastrar.Application.Dtos;
using Contato.Cadastrar.Domain.Entities;

namespace Contato.Cadastrar.Application.Interfaces;

public interface IContatoService
{
    ContatoEntity CadastrarContatoAsync(CadastrarContatoDto dto);
}