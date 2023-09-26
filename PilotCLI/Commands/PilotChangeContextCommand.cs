using PilotCLI;
using PilotCLI.Commands;
using PilotCLI.Pilot;

namespace PilotCLI.Commands;

public class PilotChangeContextCommand : ICommandHandler
{
    #region Fields
    private readonly ISettings _settings;
    private readonly PilotContext _pilotCtx;
    #endregion

    public PilotChangeContextCommand(PilotContext pilotCtx, ISettings settings)
    {
        _settings = settings;
        _pilotCtx = pilotCtx;
    }

    public void Execute(string? args) { }
}