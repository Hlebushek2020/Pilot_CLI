using Ascon.Pilot.DataClasses;
using Ascon.Pilot.Server.Api.Contracts;
using Ascon.Pilot.Transport;

namespace PilotCLI.Pilot;

public class PilotServerCallback : IServerCallback
{
    private IPilotServerCallbackListener? _listener;

    public void SetCallbackListener(IPilotServerCallbackListener listener) { _listener = listener; }

    public void NotifyChangeset(DChangeset changeset) { }
    public void NotifyOrganisationUnitChangeset(OrganisationUnitChangeset changeset) { }
    public void NotifyPersonChangeset(PersonChangeset changeset) { }
    public void NotifyDMetadataChangeset(DMetadataChangeset changeset) { }
    public void NotifySearchResult(DSearchResult searchResult) => _listener?.Notify(searchResult);
    public void NotifyGeometrySearchResult(DGeometrySearchResult searchResult) { }
    public void NotifyDNotificationChangeset(DNotificationChangeset changeset) { }
    public void NotifyCommandResult(Guid requestId, byte[] data, ServerCommandResult result) { }
    public void NotifyChangeAsyncCompleted(DChangeset changeset) { }
    public void NotifyChangeAsyncError(Guid identity, ProtoExceptionInfo exception) { }
    public void NotifyCustomNotification(string name, byte[] data) { }
    public void NotifyAccessChangeset(Guid objectId) { }
}