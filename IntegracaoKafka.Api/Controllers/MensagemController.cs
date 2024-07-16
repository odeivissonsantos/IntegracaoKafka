using IntegracaoKafka.Entities.DTOs;
using IntegracaoKafka.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntegracaoKafka.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MensagemController : ControllerBase
    {
        private readonly ILogger<MensagemController> _logger;
        private readonly MensagemService _mensagemService;

        public MensagemController(ILogger<MensagemController> logger, MensagemService mensagemService)
        {
            _logger = logger;
            _mensagemService = mensagemService;
        }

        [HttpPost]
        public async Task<IActionResult> IncluirAsync(RequestMensagemDTO mensagem)
        {
            try
            {
                return Ok(await _mensagemService.IncluirAsync(mensagem));
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EnviarAsync()
        {
            try
            {
                return Ok(await _mensagemService.EnviarAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
