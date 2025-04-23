using DotGPT.Core.Models;

namespace DotGPT.Core.Interfaces
{
    public interface ISettingsService
    {
        Task<UserSettings> GetSettingsAsync();
        Task SaveSettingsAsync(UserSettings settings);
        Task<T> GetSettingValueAsync<T>(string key, T defaultValue);
        Task SaveSettingValueAsync<T>(string key, T value);
    }
}