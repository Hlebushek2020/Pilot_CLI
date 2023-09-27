using Ascon.Pilot.DataClasses;
using Ascon.Pilot.Server.Api.Contracts;

namespace PilotCLI.Pilot;

public class PilotRepository
{
    private readonly IServerApi _serverApi;

    #region Properties
    public IReadOnlyDictionary<int, MType> Types { get; private set; }
    public IReadOnlyDictionary<Guid, MUserState> UserStates { get; private set; }
    public IReadOnlyDictionary<Guid, MUserStateMachine> StateMachines { get; private set; }
    #endregion

    public PilotRepository(IServerApi serverApi) { _serverApi = serverApi; }

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

    public void RefreshMetadata()
    {
        DMetadata metadata = _serverApi.GetMetadata(_serverApi.GetDatabaseInfo().MetadataVersion);
        Types = metadata.Types.ToDictionary(ks => ks.Id);
        UserStates = metadata.UserStates.ToDictionary(ks => ks.Id);
        StateMachines = metadata.StateMachines.ToDictionary(ks => ks.Id);
    }
}