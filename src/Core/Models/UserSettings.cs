namespace DotGPT.Core.Models
{
    public class UserSettings
    {
        // General Settings
        public bool SaveChatHistory { get; set; } = true;
        public int MaxConversationsToKeep { get; set; } = 20;
        public bool EnableNotifications { get; set; } = true;
        public bool SoundAlerts { get; set; } = false;

        // Appearance Settings
        public string Theme { get; set; } = "Light";
        public int FontSize { get; set; } = 16;
        public bool CompactMode { get; set; } = false;
        public string ColorScheme { get; set; } = "Default";

        // Privacy Settings
        public bool AllowAnonymousAnalytics { get; set; } = false;
        public bool ParticipateInImprovementProgram { get; set; } = false;
        public bool StoreChatLocally { get; set; } = true;
        public bool UsePreferenceCookies { get; set; } = true;

        // Advanced Settings
        public string ApiEndpoint { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string DefaultModel { get; set; } = "gpt-3.5-turbo";
        public double Temperature { get; set; } = 0.7;
        public bool DeveloperMode { get; set; } = false;
        
        // Ollama Settings
        public string OllamaEndpoint { get; set; } = "http://localhost:11434";
        public string SelectedOllamaModel { get; set; } = string.Empty;
    }
}