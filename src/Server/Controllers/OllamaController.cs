using DotGPT.Core.Interfaces;
using DotGPT.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DotGPT.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OllamaController : ControllerBase
    {
        private readonly IOllamaService _ollamaService;

        public OllamaController(IOllamaService ollamaService)
        {
            _ollamaService = ollamaService;
        }

        /// <summary>
        /// Gets a list of available models from the Ollama server
        /// </summary>
        [HttpGet("models")]
        public async Task<IActionResult> GetModels()
        {
            var models = await _ollamaService.GetAvailableModelsAsync();
            return Ok(models);
        }

        /// <summary>
        /// Checks if the Ollama server is available
        /// </summary>
        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var isAvailable = await _ollamaService.IsServerAvailableAsync();
            return Ok(new { isAvailable });
        }

        /// <summary>
        /// Updates the Ollama server endpoint
        /// </summary>
        [HttpPost("endpoint")]
        public IActionResult SetEndpoint([FromBody] EndpointUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Endpoint))
            {
                return BadRequest("Endpoint cannot be empty");
            }

            _ollamaService.SetEndpoint(request.Endpoint);
            return Ok(new { endpoint = _ollamaService.GetEndpoint() });
        }
    }

    public class EndpointUpdateRequest
    {
        public string Endpoint { get; set; }
    }
}