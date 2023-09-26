using System.Text;
using Newtonsoft.Json;

namespace PilotCLI;

public class Settings : ISettings
{
    private static readonly string _settingsPath = Path.Combine(Program.Directory, "settings.json");

    public IReadOnlyDictionary<string, ContextEntry> Contexts { get; } = new Dictionary<string, ContextEntry>
    {
        { "example", new ContextEntry() }
    };

    /// <summary>
    /// Loads settings from configuration file
    /// </summary>
    /// <returns>Read only settings</returns>
    public static ISettings Load() =>
        JsonConvert.DeserializeObject<Settings>(File.ReadAllText(_settingsPath, Encoding.UTF8));

    /// <summary>
    /// Checks for the existence of a configuration file. If the configuration file does not exist, it will be created.
    /// </summary>
    /// <returns><see langword="true"/> if the configuration file exists</returns>
    public static bool Availability()
    {
        if (File.Exists(_settingsPath))
            return true;

        using StreamWriter streamWriter = new StreamWriter(_settingsPath, false, Encoding.UTF8);
        streamWriter.Write(JsonConvert.SerializeObject(new Settings(), Formatting.Indented));

        return false;
    }
}