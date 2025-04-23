using DotGPT.Core.Interfaces;
using DotGPT.Core.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace DotGPT.Client.Services
{
    public class LocalStorageSettingsService : ISettingsService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string SettingsKey = "dotgpt_user_settings";

        public LocalStorageSettingsService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<UserSettings> GetSettingsAsync()
        {
            try
            {
                var settingsJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", SettingsKey);
                
                if (string.IsNullOrEmpty(settingsJson))
                {
                    return new UserSettings();
                }
                
                var settings = JsonSerializer.Deserialize<UserSettings>(settingsJson);
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
            var settingsJson = JsonSerializer.Serialize(settings);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", SettingsKey, settingsJson);
        }

        public async Task<T> GetSettingValueAsync<T>(string key, T defaultValue)
        {
            try
            {
                var settingsJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", $"dotgpt_setting_{key}");
                
                if (string.IsNullOrEmpty(settingsJson))
                {
                    return defaultValue;
                }
                
                var value = JsonSerializer.Deserialize<T>(settingsJson);
                return value != null ? value : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public async Task SaveSettingValueAsync<T>(string key, T value)
        {
            var valueJson = JsonSerializer.Serialize(value);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", $"dotgpt_setting_{key}", valueJson);
        }
    }
}