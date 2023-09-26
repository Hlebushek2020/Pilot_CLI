namespace PilotCLI.Commands.Base;

public interface ICommand
{
    string Name { get; }
    string Description { get; }
    bool Execute(CommandContext commandCtx);
    void Help();
}