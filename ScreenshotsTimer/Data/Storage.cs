using System;
using System.IO;
using System.Text.Json;

namespace ScreenshotsTimer.Data;

public record AppConfig(string? LastFolder);

public class Storage
{
    private const string AppConfigFileName = "app-config.json";

    private AppConfig? _config;

    public string? GetLastFolder()
    {
        var config = FetchConfig();

        return config.LastFolder;
    }

    public void UpdateLastFolder(string folder)
    {
        var config = FetchConfig();

        _config = config with { LastFolder = folder };

        SaveConfig(_config);
    }

    private AppConfig FetchConfig()
    {
        if (_config != null) return _config;

        _config = LoadConfig();

        return _config ?? new AppConfig(null);
    }
    
    private static AppConfig? LoadConfig()
    {
        try
        {
            string json = File.ReadAllText(AppConfigFileName);

            return JsonSerializer.Deserialize<AppConfig>(json);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static void SaveConfig(AppConfig config)
    {
        try
        {
            string json = JsonSerializer.Serialize(config);

            File.WriteAllText(AppConfigFileName, json);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}