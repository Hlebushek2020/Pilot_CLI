using Ascon.Pilot.DataClasses;
using Ascon.Pilot.Server.Api.Contracts;

namespace PilotCLI.Pilot;

public class PilotRepository : IPilotServerCallbackListener
{
    private readonly IServerApi _serverApi;
    private readonly PilotServerCallback _serverCallback;

    private TaskCompletionSource<DSearchResult> _searchCompletionSource;

    public PilotRepository(IServerApi serverApi, PilotServerCallback serverCallback)
    {
        _serverApi = serverApi;
        _serverCallback = serverCallback;
        _serverCallback.SetCallbackListener(this);
    }

    /// <summary>
    /// Gets information on the specified objects
    /// </summary>
    /// <param name="guids">Listing <see cref="System.Guid"/> for which information needs to be obtained</param>
    /// <returns>
    /// A <see cref="System.Collections.Generic.IEnumerable{T}"/> of objects <see cref="Ascon.Pilot.DataClasses.DObject"/>,
    /// where each object <see cref="Ascon.Pilot.DataClasses.DObject"/> describes an object from the database with a
    /// certain <see cref="System.Guid"/>
    /// </returns>
    public IReadOnlyList<DObject> GetObjects(IEnumerable<Guid> guids) => _serverApi.GetObjects(guids.ToArray());

    /// <summary>
    /// Gets information on the specified object
    /// </summary>
    /// <param name="guid"><see cref="System.Guid"/> for which information needs to be obtained</param>
    /// <returns>
    /// <see cref="Ascon.Pilot.DataClasses.DObject"/> describing an object from a database with a specific <see cref="System.Guid"/>
    /// </returns>
    public DObject GetObject(Guid guid) => _serverApi.GetObjects(new[] { guid }).First();

    /// <summary>
    /// Retrieves information about all types of the current database.
    /// </summary>
    /// <returns>
    /// A <see cref="System.Collections.Generic.IReadOnlyDictionary{TKey,TValue}"/> where the TKey is the name of the
    /// type, the TValue is an object <see cref="Ascon.Pilot.DataClasses.MType"/> describing this type
    /// </returns>
    public IReadOnlyDictionary<string, MType> GetTypes()
    {
        Console.WriteLine($"[{GetType().FullName}] GetTypes");
        return _serverApi.GetMetadata(_serverApi.GetDatabaseInfo().MetadataVersion).Types.ToDictionary(ks => ks.Name);
    }

    /// <summary>
    /// Retrieves information about all user states from the database.
    /// </summary>
    /// <returns>
    /// A <see cref="System.Collections.Generic.IReadOnlyDictionary{TKey,TValue}"/> where the TKey is the
    /// <see cref="System.Guid"/> of the user state, the TValue is an object <see cref="Ascon.Pilot.DataClasses.MUserState"/>
    /// describing this user state
    /// </returns>
    public IReadOnlyDictionary<Guid, MUserState> GetUserStates()
    {
        Console.WriteLine($"[{GetType().FullName}] GetUserStates");
        return _serverApi.GetMetadata(_serverApi.GetDatabaseInfo().MetadataVersion).UserStates
            .ToDictionary(ks => ks.Id);
    }

    /// <summary>
    /// Searches for objects using the specified condition and with the specified parameters
    /// </summary>
    /// <param name="searchDefinition">
    /// Object <see cref="Ascon.Pilot.DataClasses.DSearchDefinition"/> describing the search condition and search parameters
    /// </param>
    /// <returns>Object <see cref="Ascon.Pilot.DataClasses.DSearchResult"/> describing the search results</returns>
    public Task<DSearchResult> Search(DSearchDefinition searchDefinition)
    {
        Console.WriteLine($"[{GetType().FullName}] Search");
        _searchCompletionSource = new TaskCompletionSource<DSearchResult>();
        _serverApi.AddSearch(searchDefinition);
        return _searchCompletionSource.Task;
    }

    public void Notify(DSearchResult result)
    {
        try
        {
            _searchCompletionSource.SetResult(result);
        }
        catch (Exception e)
        {
            _searchCompletionSource.TrySetException(e);
        }
    }
}