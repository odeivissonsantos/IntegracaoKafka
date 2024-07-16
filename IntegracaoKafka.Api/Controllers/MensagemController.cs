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
        public IActionResult Incluir(RequestMensagemDTO mensagem)
        {
            try
            {
                return Ok(_mensagemService.Incluir(mensagem));
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Enviar()
        {
            try
            {
                return Ok(_mensagemService.Enviar());
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
