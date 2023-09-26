using Newtonsoft.Json;

namespace PilotCLI;

public class ContextEntry
{
    [JsonProperty("pilot_server_url")]
    public string PilotServerUrl { get; set; } = "http://localhost:5545";

    [JsonProperty("pilot_server_database")]
    public string PilotServerDatabase { get; set; } = string.Empty;

    [JsonProperty("pilot_server_username")]
    public string PilotServerUsername { get; set; } = string.Empty;

    [JsonProperty("pilot_server_password")]
    public string PilotServerPassword { get; set; } = string.Empty;

    [JsonProperty("pilot_server_license_code")]
    public int PilotServerLicenseCode { get; set; } = 103;

    [JsonIgnore]
    public bool PilotServerUseWindowsAuth =>
        !string.IsNullOrEmpty(PilotServerUsername) &&
        (PilotServerUsername.Contains('\\') || PilotServerUsername.Contains('@'));
}