using DotGPT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotGPT.Core.Interfaces
{
    /// <summary>
    /// Interface for interacting with Ollama API
    /// </summary>
    public interface IOllamaService
    {
        /// <summary>
        /// Gets a list of available models from the Ollama server
        /// </summary>
        /// <returns>A list of available Ollama models</returns>
        Task<List<OllamaModel>> GetAvailableModelsAsync();
        
        /// <summary>
        /// Checks if the Ollama server is running and accessible
        /// </summary>
        /// <returns>True if server is accessible, false otherwise</returns>
        Task<bool> IsServerAvailableAsync();
        
        /// <summary>
        /// Sets the endpoint for the Ollama server
        /// </summary>
        /// <param name="endpoint">The URL to the Ollama server</param>
        void SetEndpoint(string endpoint);
        
        /// <summary>
        /// Gets the current endpoint for the Ollama server
        /// </summary>
        /// <returns>The current Ollama server URL</returns>
        string GetEndpoint();
    }
}