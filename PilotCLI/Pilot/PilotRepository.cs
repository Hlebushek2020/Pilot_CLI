using Ascon.Pilot.DataClasses;
using Ascon.Pilot.Server.Api.Contracts;

namespace PilotCLI.Pilot;

public class PilotRepository
{
    private readonly IServerApi _serverApi;

    #region Properties
    /// <summary>
    /// <see cref="System.Collections.Generic.IReadOnlyDictionary{TKey, TValue}"/> of types. Where the TKey is the type
    /// identifier, and the TValue is the object <see cref="Ascon.Pilot.DataClasses.MType"/> describing this type. To
    /// update this property, use method <see cref="PilotCLI.Pilot.PilotRepository.RefreshMetadata"/>
    /// </summary>
    public IReadOnlyDictionary<int, MType> Types { get; private set; }

    /// <summary>
    /// <see cref="System.Collections.Generic.IReadOnlyDictionary{TKey, TValue}"/> of user states. Where the TKey is
    /// the user state <see cref="System.Guid"/>, and the TValue is the object <see cref="Ascon.Pilot.DataClasses.MUserState"/>
    /// describing this user state. To update this property, use method <see cref="PilotCLI.Pilot.PilotRepository.RefreshMetadata"/>
    /// </summary>
    public IReadOnlyDictionary<Guid, MUserState> UserStates { get; private set; }

    /// <summary>
    /// <see cref="System.Collections.Generic.IReadOnlyDictionary{TKey, TValue}"/> of state machines. Where the TKey is
    /// the state machine <see cref="System.Guid"/>, and the TValue is the object <see cref="Ascon.Pilot.DataClasses.MUserStateMachine"/>
    /// describing this state machine. To update this property, use method <see cref="PilotCLI.Pilot.PilotRepository.RefreshMetadata"/>
    /// </summary>
    public IReadOnlyDictionary<Guid, MUserStateMachine> StateMachines { get; private set; }
    #endregion

    public PilotRepository(IServerApi serverApi) { _serverApi = serverApi; }

    /// <summary>
    /// Gets information on the specified objects
    /// </summary>
    /// <param name="guids">Listing <see cref="System.Guid"/> for which information needs to be obtained</param>
    /// <returns>
    /// A <see cref="System.Collections.Generic.IReadOnlyList{T}"/> of objects <see cref="Ascon.Pilot.DataClasses.DObject"/>,
    /// where each object <see cref="Ascon.Pilot.DataClasses.DObject"/> describes an object from the database with a
    /// certain <see cref="System.Guid"/>
    /// </returns>
    public IReadOnlyList<DObject> GetObjects(IEnumerable<Guid> guids) => _serverApi.GetObjects(guids.ToArray());

    /// <summary>
    /// Update metadata (types, user states, and state machines)
    /// </summary>
    public void RefreshMetadata()
    {
        DMetadata metadata = _serverApi.GetMetadata(_serverApi.GetDatabaseInfo().MetadataVersion);
        Types = metadata.Types.ToDictionary(ks => ks.Id);
        UserStates = metadata.UserStates.ToDictionary(ks => ks.Id);
        StateMachines = metadata.StateMachines.ToDictionary(ks => ks.Id);
    }
}