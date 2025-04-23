using DotGPT.Core.Interfaces;
using DotGPT.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotGPT.Infrastructure.Services
{
    /// <summary>
    /// Implementation for interacting with Ollama API
    /// </summary>
    public class OllamaService : IOllamaService
    {
        private string _endpoint;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public OllamaService(HttpClient httpClient, string endpoint = "http://localhost:11434")
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _endpoint = endpoint;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        /// <inheritdoc/>
        public string GetEndpoint() => _endpoint;

        /// <inheritdoc/>
        public void SetEndpoint(string endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentException("Endpoint cannot be null or empty", nameof(endpoint));
            }
            
            _endpoint = endpoint.TrimEnd('/');
        }

        /// <inheritdoc/>
        public async Task<bool> IsServerAvailableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_endpoint}/api/version");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<List<OllamaModel>> GetAvailableModelsAsync()
        {
            var result = new List<OllamaModel>();
            
            try
            {
                var response = await _httpClient.GetAsync($"{_endpoint}/api/tags");
                
                if (!response.IsSuccessStatusCode)
                {
                    return result;
                }
                
                var content = await response.Content.ReadFromJsonAsync<OllamaTagsResponse>(_jsonOptions);
                
                if (content?.Models == null)
                {
                    return result;
                }
                
                foreach (var model in content.Models)
                {
                    var parsedModel = new OllamaModel
                    {
                        Name = model.Name,
                        Family = ParseModelFamily(model.Name),
                        Size = model.Size,
                        ModifiedAt = DateTimeOffset.FromUnixTimeMilliseconds(model.ModifiedAt).DateTime,
                        Metadata = new Dictionary<string, string>()
                    };
                    
                    // Attempt to parse parameter count and quantization from name
                    ParseModelDetails(parsedModel);
                    
                    result.Add(parsedModel);
                }
                
                return result;
            }
            catch
            {
                // Return empty list on error
                return result;
            }
        }
        
        #region Helper Methods
        
        private string ParseModelFamily(string modelName)
        {
            // Attempt to extract model family from name
            // e.g., llama2:7b -> llama2, mistral -> mistral
            var parts = modelName.Split(':', '-');
            return parts[0];
        }
        
        private void ParseModelDetails(OllamaModel model)
        {
            // Try to extract common parameter counts like 7b, 13b
            if (model.Name.Contains("7b", StringComparison.OrdinalIgnoreCase))
            {
                model.Parameters = "7B";
            }
            else if (model.Name.Contains("13b", StringComparison.OrdinalIgnoreCase))
            {
                model.Parameters = "13B";
            }
            else if (model.Name.Contains("70b", StringComparison.OrdinalIgnoreCase))
            {
                model.Parameters = "70B";
            }
            
            // Try to extract quantization info like q4_0, q4_K_M
            if (model.Name.Contains("q4", StringComparison.OrdinalIgnoreCase))
            {
                model.Quantization = "4-bit";
            }
            else if (model.Name.Contains("q5", StringComparison.OrdinalIgnoreCase))
            {
                model.Quantization = "5-bit";
            }
            else if (model.Name.Contains("q8", StringComparison.OrdinalIgnoreCase))
            {
                model.Quantization = "8-bit";
            }
        }
        
        #endregion
    }
    
    #region Response Models
    
    internal class OllamaTagsResponse
    {
        public List<OllamaModelTag> Models { get; set; }
    }
    
    internal class OllamaModelTag
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public long ModifiedAt { get; set; }
    }
    
    #endregion
}