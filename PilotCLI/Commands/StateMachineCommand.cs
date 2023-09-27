using PilotCLI.Commands.Base;
using PilotCLI.Pilot;

namespace PilotCLI.Commands;

public class StateMachineCommand : ICommand
{
    private readonly PilotContext _pilotCtx;

    public string Name { get; } = "state-machine";
    public string Description { get; }

    public StateMachineCommand(PilotContext pilotCtx) { _pilotCtx = pilotCtx; }

    public bool Execute(CommandContext commandCtx) { throw new NotImplementedException(); }
    public void Help() { throw new NotImplementedException(); }
}