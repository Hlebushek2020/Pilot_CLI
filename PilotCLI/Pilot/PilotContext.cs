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
    public bool IsInstalled => _settings != null;
    public string? ServerUrl => _settings?.PilotServerUrl;
    public string? Database => _settings?.PilotServerDatabase;
    public string? Username => _settings?.PilotServerUsername;
    public int LicenseCode => _settings?.PilotServerLicenseCode ?? -1;
    #endregion

    public PilotRepository? Repository { get; private set; }

    public void ChangeContext(ContextEntry? settings)
    {
        _settings = settings;
        _httpPilotClient = new HttpPilotClient(_settings.PilotServerUrl);
        Console.WriteLine("Connecting to the pilot ...");
        _httpPilotClient.Connect(false);
        PilotServerCallback serverCallback = new PilotServerCallback();
        IServerApi serverApi = _httpPilotClient.GetServerApi(serverCallback);
        Repository = new PilotRepository(serverApi, serverCallback);
        Console.WriteLine("Authentication in the pilot's database ...");
        IAuthenticationApi authApi = _httpPilotClient.GetAuthenticationApi();
        authApi.Login(
            _settings.PilotServerDatabase,
            _settings.PilotServerUsername,
            _settings.PilotServerPassword.EncryptAes(),
            _settings.PilotServerUseWindowsAuth,
            _settings.PilotServerLicenseCode);
        serverApi.OpenDatabase();
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