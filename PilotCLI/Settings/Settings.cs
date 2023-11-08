using System.Text;
using Newtonsoft.Json;

namespace PilotCLI;

public class Settings : ISettings
{
    [JsonIgnore]
    public static string SettingsPath { get; }

    [JsonProperty("contexts")]
    public IReadOnlyDictionary<string, ContextEntry> Contexts { get; set; } = new Dictionary<string, ContextEntry>();

    [JsonProperty("working_folder")]
    public string WorkingFolder { get; set; } = Directory.GetCurrentDirectory();

    [JsonProperty("command_signature_color")]
    public ConsoleColor CommandSignatureColor { get; set; } = ConsoleColor.DarkYellow;

    [JsonProperty("command_color")]
    public ConsoleColor CommandColor { get; set; } = ConsoleColor.DarkMagenta;

    [JsonProperty("other_text_color")]
    public ConsoleColor OtherTextColor { get; set; } = Console.ForegroundColor;

    static Settings()
    {
        try
        {
            string docFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            SettingsPath = Path.Combine(docFolderPath, "SergeyGovorunov", "PilotCLI");
            if (!Directory.Exists(SettingsPath))
                Directory.CreateDirectory(SettingsPath);
        }
        catch
        {
            SettingsPath = Program.Directory;
        }
        SettingsPath = Path.Combine(SettingsPath, "settings.json");
    }

    /// <summary>
    /// Loads settings from configuration file
    /// </summary>
    /// <returns>Read only settings</returns>
    public static ISettings Load() =>
        JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsPath, Encoding.UTF8));

    /// <summary>
    /// Checks for the existence of a configuration file. If the configuration file does not exist, it will be created.
    /// </summary>
    /// <returns><see langword="true"/> if the configuration file exists</returns>
    public static bool Availability()
    {
        if (File.Exists(SettingsPath))
            return true;

        Settings settings = new Settings
        {
            Contexts = new Dictionary<string, ContextEntry>
            {
                { "example", new ContextEntry() }
            }
        };

        using StreamWriter streamWriter = new StreamWriter(SettingsPath, false, Encoding.UTF8);
        streamWriter.Write(JsonConvert.SerializeObject(settings, Formatting.Indented));

        return false;
    }
}