using System;
using System.Collections.Generic;

namespace DotGPT.Core.Models
{
    /// <summary>
    /// Represents an Ollama model available on the server
    /// </summary>
    public class OllamaModel
    {
        /// <summary>
        /// The name of the model
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The model family (e.g., llama, mistral)
        /// </summary>
        public string Family { get; set; }
        
        /// <summary>
        /// The size of the model in bytes
        /// </summary>
        public long Size { get; set; }
        
        /// <summary>
        /// The model's parameter count, if available
        /// </summary>
        public string Parameters { get; set; }
        
        /// <summary>
        /// The quantization level of the model, if applicable
        /// </summary>
        public string Quantization { get; set; }
        
        /// <summary>
        /// The last modified date of the model
        /// </summary>
        public DateTime ModifiedAt { get; set; }
        
        /// <summary>
        /// Additional metadata about the model as key-value pairs
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}