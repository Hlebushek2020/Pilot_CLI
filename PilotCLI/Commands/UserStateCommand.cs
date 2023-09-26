using PilotCLI.Commands.Base;

namespace PilotCLI.Commands;

public class UserStateCommand : ICommand
{
    public string Name { get; } = "user-state";
    public string Description { get; }
    public bool Execute(CommandContext commandCtx) { throw new NotImplementedException(); }
    public void Help() { throw new NotImplementedException(); }
}