using Contato.Cadastrar.Application.Dtos;
using Contato.Cadastrar.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Contato.Cadastrar.Web.Controllers;

[Route("api/[controller]")]
public class ContatoController : ControllerBase
{
    private readonly ILogger<ContatoController> _logger;
    private readonly IContatoService _contatoService;

    public ContatoController(ILogger<ContatoController> logger, IContatoService contatoService)
    {
        _logger = logger;
        _contatoService = contatoService;
    }


    /// <summary>
    /// Enviar um contato para a fila de cadastro.
    /// </summary>
    /// <param name="dto">Dados do contato a serem cadastrados.</param>
    /// <returns>Retorna o ID do contato a ser criado e a data da requisição.</returns>
    /// <response code="200">Contato enviado para fila de cadastro com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost("[action]")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CadastrarContato([FromBody] CadastrarContatoDto dto)
    {
        try
        {
            _logger.LogInformation($"Acessou {nameof(CadastrarContato)}. Entrada: {dto}");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Dados inválidos - Entrada: {dto}");
                return BadRequest(ModelState);
            }

            var contato = _contatoService.CadastrarContatoAsync(dto);
            _logger.LogInformation($"Dados enviados para fila com sucesso.");

            var response = new ResponseDto()
            {
                Id = contato.Id,
                DataCriacao = DateTime.Now
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Falha na api CadastrarContato. Erro{ex}");
            return StatusCode(500, $"Internal server error - {ex}");
        }
    }
}