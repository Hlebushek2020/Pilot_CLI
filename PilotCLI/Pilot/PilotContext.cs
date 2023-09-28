using Ascon.Pilot.Common;
using Ascon.Pilot.DataClasses;
using Ascon.Pilot.Server.Api;
using Ascon.Pilot.Server.Api.Contracts;

namespace PilotCLI.Pilot;

public class PilotContext : IDisposable
{
    private ContextEntry? _settings;
    private HttpPilotClient? _httpPilotClient;

    #region Properties
    /// <summary>
    /// Indicates whether a connection to any database is established or not
    /// </summary>
    public bool IsInstalled => _settings != null;

    /// <summary>
    /// Current server url
    /// </summary>
    public string? ServerUrl => _settings?.PilotServerUrl;

    /// <summary>
    /// Current database
    /// </summary>
    public string? Database => _settings?.PilotServerDatabase;

    /// <summary>
    ///  Current user account for a given database
    /// </summary>
    public string? Username => _settings?.PilotServerUsername;

    /// <summary>
    /// Current license type
    /// </summary>
    public int LicenseCode => _settings?.PilotServerLicenseCode ?? -1;
    #endregion

    public PilotRepository? Repository { get; private set; }

    /// <summary>
    /// Connects to another database with the specified settings
    /// </summary>
    /// <param name="settings">Settings for connecting to another database</param>
    public void ChangeContext(ContextEntry? settings)
    {
        _settings = settings;

        if (_httpPilotClient != null)
        {
            Console.WriteLine("Disconnect to the pilot ...");
            _httpPilotClient.Disconnect();
        }

        _httpPilotClient = new HttpPilotClient(_settings.PilotServerUrl);
        Console.WriteLine("Connecting to the pilot ...");
        _httpPilotClient.Connect(false);
        PilotServerCallback serverCallback = new PilotServerCallback();
        IServerApi serverApi = _httpPilotClient.GetServerApi(serverCallback);
        Repository = new PilotRepository(serverApi);
        Console.WriteLine("Authentication in the pilot's database ...");
        IAuthenticationApi authApi = _httpPilotClient.GetAuthenticationApi();
        authApi.Login(
            _settings.PilotServerDatabase,
            _settings.PilotServerUsername,
            _settings.PilotServerPassword.EncryptAes(),
            _settings.PilotServerUseWindowsAuth,
            _settings.PilotServerLicenseCode);
        serverApi.OpenDatabase();
        Repository.RefreshMetadata();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _httpPilotClient?.Disconnect();
            _httpPilotClient?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}