using Newtonsoft.Json;

namespace PilotCLI;

/// <summary>
/// Represents connection parameters
/// </summary>
public class ContextEntry
{
    /// <summary>
    /// Pilot server url
    /// </summary>
    [JsonProperty("pilot_server_url")]
    public string PilotServerUrl { get; set; } = "http://localhost:5545";

    /// <summary>
    /// Pilot database
    /// </summary>
    [JsonProperty("pilot_server_database")]
    public string PilotServerDatabase { get; set; } = string.Empty;

    /// <summary>
    /// User account for a given Pilot database
    /// </summary>
    [JsonProperty("pilot_server_username")]
    public string PilotServerUsername { get; set; } = string.Empty;

    /// <summary>
    /// User account password
    /// </summary>
    [JsonProperty("pilot_server_password")]
    public string PilotServerPassword { get; set; } = string.Empty;

    /// <summary>
    /// License type: Pilot ICE: 100; Pilot ECM: 101; Pilot Enterprise: 103.
    /// </summary>
    [JsonProperty("pilot_server_license_code")]
    public int PilotServerLicenseCode { get; set; } = 103;

    /// <summary>
    /// true is using windows authorization
    /// </summary>
    [JsonIgnore]
    public bool PilotServerUseWindowsAuth =>
        !string.IsNullOrEmpty(PilotServerUsername) &&
        (PilotServerUsername.Contains('\\') || PilotServerUsername.Contains('@'));
}