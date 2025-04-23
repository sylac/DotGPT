using DotGPT.Core.Interfaces;
using DotGPT.Core.Models;
using System.Text.Json;

namespace DotGPT.Infrastructure.Services
{
    /// <summary>
    /// Server-side implementation of ISettingsService that stores settings in a JSON file
    /// This follows Clean Architecture by providing infrastructure functionality without UI dependencies
    /// </summary>
    public class JsonFileSettingsService : ISettingsService
    {
        private readonly string _settingsFilePath;
        private readonly string _settingsFolder;

        public JsonFileSettingsService(string settingsFolder = null)
        {
            // Default to application data folder if not specified
            _settingsFolder = settingsFolder ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                "DotGPT");
            
            // Ensure the directory exists
            if (!Directory.Exists(_settingsFolder))
            {
                Directory.CreateDirectory(_settingsFolder);
            }
            
            _settingsFilePath = Path.Combine(_settingsFolder, "settings.json");
        }

        public async Task<UserSettings> GetSettingsAsync()
        {
            try
            {
                if (!File.Exists(_settingsFilePath))
                {
                    return new UserSettings();
                }

                var json = await File.ReadAllTextAsync(_settingsFilePath);
                var settings = JsonSerializer.Deserialize<UserSettings>(json);
                return settings ?? new UserSettings();
            }
            catch
            {
                // If there's any error, return default settings
                return new UserSettings();
            }
        }

        public async Task SaveSettingsAsync(UserSettings settings)
        {
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            
            await File.WriteAllTextAsync(_settingsFilePath, json);
        }

        public async Task<T> GetSettingValueAsync<T>(string key, T defaultValue)
        {
            try
            {
                string filePath = Path.Combine(_settingsFolder, $"{key}.json");
                
                if (!File.Exists(filePath))
                {
                    return defaultValue;
                }

                var json = await File.ReadAllTextAsync(filePath);
                var value = JsonSerializer.Deserialize<T>(json);
                return value != null ? value : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public async Task SaveSettingValueAsync<T>(string key, T value)
        {
            string filePath = Path.Combine(_settingsFolder, $"{key}.json");
            var json = JsonSerializer.Serialize(value);
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}