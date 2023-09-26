using Ascon.Pilot.Common;
using Ascon.Pilot.DataClasses;
using Ascon.Pilot.Server.Api;
using Ascon.Pilot.Server.Api.Contracts;

namespace PilotCLI.Pilot;

public class PilotContext : IDisposable
{
    private ISettings? _settings;
    private HttpPilotClient? _httpPilotClient;

    public PilotRepository? Repository { get; private set; }

    public void ChangeContext(ISettings settings)
    {
        _settings = settings;
        //_httpPilotClient = new HttpPilotClient(_settings.PilotServerUrl);
    }

    /// <summary>
    /// Connects to the pilot database with the specified parameters in the settings. After this, property
    /// <see cref="DataSenderToAlfa.Pilot.PilotContext.Repository"/> will become available.
    /// </summary>
    /// <returns>
    /// <see cref="Ascon.Pilot.DataClasses.DDatabaseInfo"/> object representing information about the database.
    /// </returns>
    public DDatabaseInfo Connect()
    {
        Console.WriteLine($"[{GetType().FullName}] Connecting to the pilot ...");
        _httpPilotClient.Connect(false);
        PilotServerCallback serverCallback = new PilotServerCallback();
        IServerApi serverApi = _httpPilotClient.GetServerApi(serverCallback);
        Repository = new PilotRepository(serverApi, serverCallback);

        Console.WriteLine($"[{GetType().FullName}] Authentication in the pilot's database ...");
        IAuthenticationApi authApi = _httpPilotClient.GetAuthenticationApi();
       /* authApi.Login(
            _settings.PilotServerDatabase,
            _settings.PilotServerUsername,
            _settings.PilotServerPassword.EncryptAes(),
            _settings.PilotServerUseWindowsAuth,
            _settings.PilotServerLicenseCode);*/
        return serverApi.OpenDatabase();
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