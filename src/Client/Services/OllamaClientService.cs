using DotGPT.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotGPT.Client.Services
{
    /// <summary>
    /// Client-side service for interacting with Ollama API through the backend
    /// </summary>
    public class OllamaClientService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public OllamaClientService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Gets a list of available models from the Ollama server
        /// </summary>
        public async Task<List<OllamaModel>> GetAvailableModelsAsync()
        {
            try
            {
                var models = await _httpClient.GetFromJsonAsync<List<OllamaModel>>("api/ollama/models", _jsonOptions);
                return models ?? new List<OllamaModel>();
            }
            catch (Exception)
            {
                // Return empty list on error
                return new List<OllamaModel>();
            }
        }

        /// <summary>
        /// Checks if the Ollama server is available
        /// </summary>
        public async Task<bool> IsServerAvailableAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<StatusResponse>("api/ollama/status", _jsonOptions);
                return response?.IsAvailable ?? false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Updates the Ollama server endpoint
        /// </summary>
        public async Task<bool> UpdateEndpointAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/ollama/endpoint", new { Endpoint = endpoint });
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private class StatusResponse
        {
            public bool IsAvailable { get; set; }
        }
    }
}