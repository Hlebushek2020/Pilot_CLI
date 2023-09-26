using PilotCLI.Commands.Base;

namespace PilotCLI.Commands;

public class TypeCommand : ICommand
{
    public string Name { get; } = "type";
    public string Description { get; }
    public bool Execute(CommandContext commandCtx) { throw new NotImplementedException(); }
    public void Help() { throw new NotImplementedException(); }
}